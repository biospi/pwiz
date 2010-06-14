//
// $Id$
//
// Original author: Robert Burke <robert.burke@proteowizard.org>
//
// Copyright 2010 Spielberg Family Center for Applied Proteomics
//   University of Southern California, Los Angeles, California  90033
//
// Licensed under the Apache License, Version 2.0 (the "License"); 
// you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at 
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software 
// distributed under the License is distributed on an "AS IS" BASIS, 
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
// See the License for the specific language governing permissions and 
// limitations under the License.
//

#define PWIZ_SOURCE

#include <typeinfo>
#include <sstream>
#include <iostream>
#include <stdexcept>
#include <boost/tokenizer.hpp>
#include <boost/foreach.hpp>
#include "KwCVMap.hpp"

namespace pwiz{
namespace mziddata{

using namespace std;
using namespace boost;
using namespace pwiz::cv;

//
// struct CVMap
//

CVMap::CVMap()
    : keyword(), cvid(cv::CVID_Unknown), path("/")
{
}

CVMap::CVMap(const string& keyword, CVID cvid, const string& path)
    : keyword(keyword), cvid(cvid), path(path)
{
}

CVMap* CVMap::createMap(const vector<string>& quad) 
{
    CVMap* map;

    if (quad.size() < 4)
        throw runtime_error("[CVMap::createMap] Too few elements in createMap quad");

    CVID cvid = cvTermInfo(quad[2]).cvid;
    string path;
    if (quad[3].size() == 0)
        throw runtime_error("[CVMap::createMap] Blank path.");
    path = quad[3];

    if (cvid == CVID_Unknown)
        throw runtime_error(("[CVMap::createMap] Unknown CVID: "+quad[2]).c_str());
    
    if (quad[0] == "plain")
        map = new CVMap(quad[1], cvid, path);
    else if (quad[0] == "regex")
        map = new RegexCVMap(quad[1], cvid, path);
    else
        throw runtime_error(("[CVMap::createMap] Unknown map: "+quad[0]).c_str());
    
    return map;
}

const char* CVMap::getTag() const
{
    return "plain";
}

bool CVMap::operator()(const string& text) const
{
    return keyword == text;
}

bool CVMap::operator==(const CVMap& right) const
{
    return keyword == right.keyword &&
        cvid == right.cvid &&
        path == right.path;
}

//
// struct RegexCVMap
//

RegexCVMap::RegexCVMap()
    : CVMap(".*", CVID_Unknown, "/"), pattern(".*")
{
}

RegexCVMap::RegexCVMap(const string& pattern, CVID cvid, const string& path)
    : CVMap(pattern, cvid, path), pattern(pattern)
{
}

RegexCVMap::~RegexCVMap()
{
}

cmatch RegexCVMap::match(std::string& text)
{
    cmatch what;

    regex_match(text.c_str(), what, pattern);

    return what;
}

const char* RegexCVMap::getTag() const
{
    return "regex";
}

void RegexCVMap::setPattern(const std::string& pattern)
{
    keyword = pattern;
    this->pattern = regex(pattern);
}

bool RegexCVMap::operator()(const string& text) const
{
    cmatch what;
    if (regex_match(text.c_str(), what, pattern))
    {
        return true;
    }
    
    return false;
}

//
// struct StringMatchCVMap
//

StringMatchCVMap::StringMatchCVMap(const string& keyword)
    : CVMap(keyword, CVID_Unknown, "/")
{
}

bool StringMatchCVMap::operator()(const CVMap& that) const
{
    return (*this) == that;
}

bool StringMatchCVMap::operator()(const CVMapPtr& that) const
{
    return (*this) == that;
}

bool StringMatchCVMap::operator==(const CVMap& right) const
{
    return keyword == right.keyword;
}

bool StringMatchCVMap::operator==(const CVMapPtr& right) const
{
    return right.get() && keyword == right->keyword;
}

//
// struct CVIDMatchCVMap
//

CVIDMatchCVMap::CVIDMatchCVMap(CVID cvid)
    : CVMap("", cvid, "/")
{
}

bool CVIDMatchCVMap::operator()(const CVMap& right) const
{
    return (*this) == right;
}

bool CVIDMatchCVMap::operator()(const CVMapPtr& right) const
{
    return (*this) == right;
}

bool CVIDMatchCVMap::operator==(const CVMap& right) const
{
    return cvid == right.cvid;
}

bool CVIDMatchCVMap::operator==(const CVMapPtr& right) const
{
    return cvid == right->cvid;
}

//
// operators
//

ostream& operator<<(ostream& os, const CVMap& cm)
{
    string id = typeid(cm).name();
    
    os << cm.getTag() << "\t" << cm.keyword
       << "\t" << cvTermInfo(cm.cvid).id
       << "\t" << cm.path
       << "\n";

    return os;
}

ostream& operator<<(ostream& os, const CVMapPtr cmp)
{
    if (cmp.get())
        return os << (*cmp);

    return os;
}

ostream& operator<<(ostream& os, const CVMap* cmp)
{
    if (cmp)
        return os << (*cmp);

    return os;
}

istream& operator>>(istream& is, CVMapPtr& cm)
{
    string line;
    getline(is, line);

    if (!line.size())
        throw length_error("empty line found where record expected.");
    
    vector<string> tokens;
    char_separator<char> delim("\t");
    typedef tokenizer< char_separator<char> > tab_tokenizer;

    // Step 1: explode the line.
    tab_tokenizer tokes(line, delim);
    for (tab_tokenizer::iterator t=tokes.begin(); t!=tokes.end(); t++)
    {
        tokens.push_back(*t);
    }

    // Step 2: verify the # of fields.
    if (tokens.size()<4)
    {
        ostringstream err;
        err << "Too few fields (" << tokens.size()
            << ") in line: " << line;
        throw runtime_error(err.str().c_str());
    }

    // Step 3: Call the factory method to get a *Ptr object
    CVMap* map = CVMap::createMap(tokens);

    if (map)
        cm = CVMapPtr(map);
    else
        // Might want to find a more descriptive error to throw.
        throw runtime_error("No CVMap available");

    return is;
}

ostream& operator<<(ostream& os, const vector<CVMapPtr>& cmVec)
{
    for (vector<CVMapPtr>::const_iterator i=cmVec.begin();
         i != cmVec.end(); i++)
    {
        os << (*i);
    }

    return os;
}

istream& operator>>(istream& is, vector<CVMapPtr>& cmVec)
{
    while(is)
    {
        try {
            CVMapPtr ptr;
            is >> ptr;
            cmVec.push_back(ptr);
        }
        catch(length_error le)
        {
            // This occurs after the last record has been read.
        }
    }

    return is;
}

} // namespace mziddata
} // namespace pwiz
