//
// $Id$
//
//
// Original author: Barbara Frewen <frewen@u.washington.edu>
//
// Copyright 2012 University of Washington - Seattle, WA 98195
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

/*
 *class definition for PepXMLreader.h
 */

#include "PepXMLreader.h"
#include "BlibMaker.h"
#include <algorithm>
#include <boost/algorithm/string.hpp>
#include <cmath>

using namespace std;
namespace bal = boost::algorithm;

namespace BiblioSpec {

static const int STATE_INIT = -1;
static const int STATE_ROOT = 0;
static const int STATE_PROPHET_SUMMARY = 1;
static const int STATE_SEARCH_HIT_BEST = 5;
static const int STATE_SEARCH_HIT_BEST_SEEN = 6;

PepXMLreader::PepXMLreader(BlibBuilder& maker,
                           const char* xmlfilename,
                           const ProgressIndicator* parentProgress)
: BuildParser(maker, xmlfilename, parentProgress),
  analysisType_(UNKNOWN_ANALYSIS),
  scoreType_(PEPTIDE_PROPHET_SOMETHING),
  lastFilePosition_(0),
  state(STATE_INIT)
{
    this->setFileName(xmlfilename); // this is for the saxhandler
    numFiles = 0;
    pepProb = 0;
    probCutOff = getScoreThreshold(PEPXML);
    dirs.push_back("../");   // look in parent dir in addition to cwd
    dirs.push_back("../../");  // look in grandparent dir in addition to cwd
    extensions.push_back(".mzML"); // look for spec in mzML files
    extensions.push_back(".mzXML"); // look for spec in mzXML files
}

PepXMLreader::~PepXMLreader() {
}

void PepXMLreader::startElement(const XML_Char* name, const XML_Char** attr)
{
   // Make sure this is actually a pepXML file and not some other XML format
   // to a pepXML extension (.pep.xml or .pepXML).
   if(state == STATE_INIT) {
       if (!isElement("msms_pipeline_analysis", name)) {
           throw BlibException(false, "Invalid pepXML root tag '%s' must be 'msms_pipeline_analysis'.",
                               name);
       }
       state = STATE_ROOT;
   } else if(isElement("peptideprophet_summary",name)) {
       analysisType_ = PEPTIDE_PROPHET_ANALYSIS;
       state = STATE_PROPHET_SUMMARY;
   } else if(isElement("analysis_summary", name)) {
       string analysis = getAttrValue("analysis", attr);
       
       if (analysis.find("interprophet") == 0) {
           analysisType_ = INTER_PROPHET_ANALYSIS;
           // Unfortunately, there is no way to get a file count
           // from this element.
           struct stat filestatus;
           stat(getFileName().c_str(), &filestatus);
           // work in bytes / 1000 to avoid overflow
           initSpecFileProgress(filestatus.st_size / 1000);
       }
   } else if(state == STATE_PROPHET_SUMMARY && isElement("inputfile",name)) {
      // Count files for use in reporting percent complete
      numFiles++;
   } else if(isElement("msms_run_summary",name)) {
      string fileroot = getRequiredAttrValue("base_name",attr);
      // Because Mascot2XML uses the full path for the base_name,
      // only the part beyond the last "\" or "/" is taken.
      size_t slash = fileroot.rfind('/');
      size_t bslash = fileroot.rfind('\\');
      if (slash == string::npos || (bslash != string::npos && bslash > slash))
          slash = bslash;
      if (slash != string::npos)
          fileroot.erase(0, slash + 1);
          
      setSpecFileName(fileroot.c_str(), extensions, dirs); 

      // Check if this pepXML file is from Proteome Discoverer
      string rawType = getAttrValue("raw_data_type", attr);
      if (rawType == ".msf") {
          Verbosity::comment(V_DEBUG, "Pepxml file is from Proteome Discoverer.");
          analysisType_ = PROTEOME_DISCOVERER_ANALYSIS;
      }
   }

   //get massType and search engine
   else if(isElement("search_summary",name)) {
       if (analysisType_ == UNKNOWN_ANALYSIS ||
           analysisType_ == PROTEOME_DISCOVERER_ANALYSIS) {
           string search_engine = getAttrValue("search_engine", attr);
           std::transform(search_engine.begin(), search_engine.end(), search_engine.begin(), ::tolower);
           if(search_engine.find("spectrum mill") == 0) {
               Verbosity::comment(V_DEBUG, "Pepxml file is from Spectrum Mill.");
               analysisType_ = SPECTRUM_MILL_ANALYSIS;
               scoreType_ = SPECTRUM_MILL;
               probCutOff = 0; // accept all psms

               lookUpBy_ = INDEX_ID; 
               specReader_->setIdType(INDEX_ID);
           } else if(search_engine.find("omssa") == 0) {
               Verbosity::debug("Pepxml file is from OMSSA.");
               analysisType_ = OMSSA_ANALYSIS;
               scoreType_ = OMSSA_EXPECTATION_SCORE;
               probCutOff = getScoreThreshold(OMSSA);
           } else if(search_engine.find("protein prospector search compare") == 0) {
               Verbosity::comment(V_DEBUG, "Pepxml file is from Protein Prospector.");
               analysisType_ = PROTEIN_PROSPECTOR_ANALYSIS;
               scoreType_ = PROTEIN_PROSPECTOR_EXPECT;
               probCutOff = getScoreThreshold(PROT_PROSPECT);
           } else if(search_engine.find("morpheus") == 0) {
               Verbosity::comment(V_DEBUG, "Pepxml file is from Morpheus.");
               analysisType_ = MORPHEUS_ANALYSIS;
               scoreType_ = MORPHEUS_SCORE;
               probCutOff = getScoreThreshold(MORPHEUS);

               lookUpBy_ = INDEX_ID; 
               specReader_->setIdType(INDEX_ID);
           } else if(search_engine.find("ms-gfdb") == 0) {
               Verbosity::comment(V_DEBUG, "Pepxml file is from MS-GFDB.");
               analysisType_ = MSGF_ANALYSIS;
               scoreType_ = MSGF_SCORE;
               probCutOff = getScoreThreshold(MSGF);

               lookUpBy_ = NAME_ID;
               specReader_->setIdType(NAME_ID);
           } else if (search_engine.find("peaks db") == 0) {
               Verbosity::comment(V_DEBUG, "Pepxml file is from PEAKS");
               analysisType_ = PEAKS_ANALYSIS;
               scoreType_ = PEAKS_CONFIDENCE_SCORE;
               probCutOff = getScoreThreshold(PEAKS);
           } else if(search_engine.find("sequest") == 0 &&
                     analysisType_ == PROTEOME_DISCOVERER_ANALYSIS) {
               Verbosity::comment(V_DEBUG, "Pepxml file is from SEQUEST Proteome Discoverer.");
               scoreType_ = PERCOLATOR_QVALUE;
               probCutOff = getScoreThreshold(SQT);
           } else if(search_engine.find("mascot") == 0 &&
                     analysisType_ == PROTEOME_DISCOVERER_ANALYSIS) {
               Verbosity::comment(V_DEBUG, "Pepxml file is from Mascot Proteome Discoverer.");
               scoreType_ = MASCOT_IONS_SCORE;
               probCutOff = getScoreThreshold(MASCOT);
           } else if(search_engine.find("x! tandem") == 0 &&
                     analysisType_ != PEPTIDE_PROPHET_ANALYSIS) {
               Verbosity::comment(V_DEBUG, "Pepxml file is from X! Tandem.");
               analysisType_ = XTANDEM_ANALYSIS;
               scoreType_ = TANDEM_EXPECTATION_VALUE;
               probCutOff = getScoreThreshold(TANDEM);
           }// else assume peptide prophet or inter prophet 

           if (analysisType_ == PROTEOME_DISCOVERER_ANALYSIS &&
               scoreType_ != PERCOLATOR_QVALUE &&
               scoreType_ != MASCOT_IONS_SCORE) {
               throw BlibException(false, "The .pep.xml file appears to be from "
                                "Proteome Discoverer but not from one of the supported "
                                "search engines (SEQUEST, Mascot).");

           }

       }
       
       if (boost::iequals("average", getAttrValue("fragment_mass_type", attr)))
           massType = 0;
       else
           massType = 1;
       
       AminoAcidMasses::initializeMass(aminoacidmass, massType);

   } else if(isElement("spectrum_query", name)) {
       // is it better to do this at the start of the element or the end?
       scanIndex=-1;
       scanNumber=-1;
       charge=0;
       precursorMZ=0;
       pepProb = 0;
       pepSeq[0]='\0';
       mods.clear();
       
       int minCharge = 1;
       
       // if spectrum mill type
       if (analysisType_ == SPECTRUM_MILL_ANALYSIS) {
           spectrumName = getRequiredAttrValue("spectrum", attr);
           minCharge = 0;
           // In case this is necessary for calculating charge
           precursorMZ = atof(getAttrValue("precursor_m_over_z", attr));
       }
       // if morpheus type
       else if (analysisType_ == MORPHEUS_ANALYSIS) {
           spectrumName = getRequiredAttrValue("spectrum", attr);
           scanNumber = getIntRequiredAttrValue("start_scan", attr);
           scanIndex = scanNumber - 1;
       }
       // this should never happen, error should have been thrown earlier
       else if (analysisType_ == UNKNOWN_ANALYSIS) {
           throw BlibException(false, "The .pep.xml file is not from one of the recognized sources");
       // if any other type
       } else {
           scanNumber = getIntRequiredAttrValue("start_scan", attr);
           if (lookUpBy_ == NAME_ID) {
               spectrumName = getRequiredAttrValue("spectrumNativeID", attr);
           } else if (psms_.empty() && !(spectrumName = getAttrValue("spectrumNativeID", attr)).empty()) {
               // if the file has spectrumNativeIDs, use those for spectrum lookups
               lookUpBy_ = NAME_ID;
           }
       }
       charge = getIntRequiredAttrValue("assumed_charge",attr, minCharge, 20);
   } else if(isElement("search_hit", name)) {
       // Only use this search hit, if rank is 1 or missing (zero)
       if (atoi(getAttrValue("hit_rank",attr)) < 2 && state == STATE_ROOT) {
           strcpy(pepSeq,getRequiredAttrValue("peptide",attr));
           if(charge == 0) {
               if(precursorMZ == 0 || *getAttrValue("parentCharge",attr) != '\0')
                   charge = getIntRequiredAttrValue("parentCharge",attr, 1, 10);
               else {
                   // If all else fails with Spectrum Mill, use the pecursor m/z and
                   // neutral mass to calculate the charge.
                   double neutralMass = getDoubleRequiredAttrValue("calc_neutral_pep_mass",attr);
                   charge = (int)((neutralMass / precursorMZ) + 0.5);
               }
           }
           state = STATE_SEARCH_HIT_BEST;
       }
   } else if(isElement("modification_info", name) && state == STATE_SEARCH_HIT_BEST) {
       if (strcmp(getAttrValue("mod_nterm_mass", attr), "") != 0) {
           SeqMod curmod;
           curmod.position = 1;
           curmod.deltaMass = getDoubleRequiredAttrValue("mod_nterm_mass", attr) -
               aminoacidmass['h']; // H
           mods.push_back(curmod);
       }
       if (strcmp(getAttrValue("mod_cterm_mass", attr), "") != 0) {
           SeqMod curmod;
           curmod.position = (int)strlen(pepSeq);
           curmod.deltaMass = getDoubleRequiredAttrValue("mod_cterm_mass", attr) -
               aminoacidmass['o'] - aminoacidmass['h']; // OH
           mods.push_back(curmod);
       }
   } else if(isElement("mod_aminoacid_mass", name) && state == STATE_SEARCH_HIT_BEST) {
       // position is 1-based
       modPosition = getIntRequiredAttrValue("position",attr);
       modMass = getDoubleRequiredAttrValue("mass",attr);
       
       SeqMod curmod;
       curmod.position = modPosition;
       if( (int)strlen(pepSeq) < modPosition || modPosition < 1 ) {
           throw BlibException(false, "Cannot modify sequence %s at position "
                               "%i which is beyond its length (%i).", 
                               pepSeq, modPosition-1, strlen(pepSeq));
       }
       
       curmod.deltaMass = modMass - aminoacidmass[(int)pepSeq[modPosition-1]];

       mods.push_back(curmod);
   } else if((analysisType_ == PEPTIDE_PROPHET_ANALYSIS && isElement("peptideprophet_result", name)) ||
             (analysisType_ == INTER_PROPHET_ANALYSIS && isElement("interprophet_result", name))) {
       pepProb = getDoubleRequiredAttrValue("probability",attr);
   } else if(state == STATE_SEARCH_HIT_BEST && isElement("search_score", name)) {
       string score_name = getAttrValue("name", attr);
       bal::to_lower(score_name);

       if (score_name == "expect" ||
           (analysisType_ == PROTEOME_DISCOVERER_ANALYSIS && scoreType_ == SEQUEST_XCORR && score_name == "q-value") ||
           (analysisType_ == PROTEOME_DISCOVERER_ANALYSIS && scoreType_ == MASCOT_IONS_SCORE && score_name == "exp-value") ||
           (analysisType_ == MORPHEUS_ANALYSIS && score_name == "psm q-value") ||
           (analysisType_ == MSGF_ANALYSIS && score_name == "qvalue")) {
           pepProb = getDoubleRequiredAttrValue("value", attr);
       } else if (analysisType_ == PEAKS_ANALYSIS && score_name == "-10lgP") {
           pepProb = getDoubleRequiredAttrValue("value", attr);
           // Reverse -10 log p transform
           pepProb = pow(10, pepProb / -10.0);
       }
   }
   // no score for spectrum mill
   // mascot score is ??
}

void PepXMLreader::endElement(const XML_Char* name)
{
    if(isElement("peptideprophet_summary",name)) {
        state = STATE_ROOT;
        // now we know either the number of files or the pepxml file size
        if( numFiles > 1 ){
            initSpecFileProgress(numFiles);

        }
    } else if(isElement("msms_run_summary", name)) {
        if( analysisType_ == UNKNOWN_ANALYSIS ){
            throw BlibException(false, "The .pep.xml file is not from one of "
                                "the recognized sources (PeptideProphet, "
                                "iProphet, SpectrumMill, OMSSA, Protein Prospector, "
                                "X! Tandem, Proteome Discoverer, Morpheus, MSGF+).");
        }
        // if we are using pep.xml from Spectrum mill, we still don't have
        // scan numbers/indexes, here's a hack to get them
        if( analysisType_ == SPECTRUM_MILL_ANALYSIS ) {
            findScanIndexFromName();
        }
        // file progress will be incremented in buildTables
        // if we are counting bytes instead of number of spec files
        // indicate how far we have progressed
        if( analysisType_ == INTER_PROPHET_ANALYSIS ){
            int position = getCurrentByteIndex() / 1000;
            int progress = position - lastFilePosition_ ;
            lastFilePosition_ = position ;
            setNextProgressSize(progress);
        }

        buildTables(scoreType_);
        
        // reset values for next
        mzXMLFile[0]='\0';
        massType = 1;
        for(int i=0; i<128; i++) {
            aminoacidmass[i]=0;
        }
    } else if(isElement("spectrum_query", name)) {
        // check that we found all the values we need
        // charge and scanNumber were required attributes of spectrum_query
        // mods and spectrumName optional
        // no prob for spectrum mill
        // if prob not found for pep proph or mascot, default value is -1 and the psm will quietly be ignored
        // mascot has spectra with no peptides, but could report warning if spectrum mill or peptide prophet don't have a peptide sequence
        if( scorePasses(pepProb) && (int)strlen(pepSeq) > 0) {
            curPSM_ = new PSM();
            curPSM_->charge = charge;
            curPSM_->unmodSeq = pepSeq;
            if (scanIndex >= 0) {
                curPSM_->specIndex = scanIndex;
            }
            curPSM_->specKey = scanNumber;
            curPSM_->score = pepProb;
            curPSM_->mods = mods;
            curPSM_->specName = spectrumName;
            
            Verbosity::comment(V_DETAIL, "Adding psm.  Scan %d, charge %d, "
                               "score %.2f, seq %s, name %s.",
                               scanNumber, charge, pepProb, pepSeq,
                               spectrumName.c_str());
             psms_.push_back(curPSM_);
            curPSM_ = NULL;
        }

        // reset for next query
        // TODO (BF: Aug-13-09): would be faster/cleaner if we just kept an object at curPsm and filled values into that instead of using charge,pepSeq, etc.
        // this is reset at the beginning of the element, which is better?
        charge = 0;
        precursorMZ = 0;
        pepSeq[0] = '\0';
        scanIndex = -1;
        scanNumber = -1;
        pepProb = 0;
        spectrumName.clear();
        
        mods.clear();
        state = STATE_ROOT;
    } else if(isElement("search_hit", name)) {
        if (state == STATE_SEARCH_HIT_BEST)
            state = STATE_SEARCH_HIT_BEST_SEEN;
    }
}

bool PepXMLreader::parseFile()
{
   return parse();
}

/**
 * Decide if the score passes the required threshold for the given
 * search analysis.
 * \returns True if score passes and PSM should be included in
 * library, else false.
 */
bool PepXMLreader::scorePasses(double score){
    switch(analysisType_){
    case PEPTIDE_PROPHET_ANALYSIS:
    case INTER_PROPHET_ANALYSIS:
        if(score >= probCutOff){
            return true;
        }
        break;

    case SPECTRUM_MILL_ANALYSIS:
        return true;

    case OMSSA_ANALYSIS:
    case PROTEIN_PROSPECTOR_ANALYSIS:
    case PROTEOME_DISCOVERER_ANALYSIS:
    case XTANDEM_ANALYSIS:
    case MORPHEUS_ANALYSIS:
    case MSGF_ANALYSIS:
    case PEAKS_ANALYSIS:
        if(score < probCutOff){
            return true;
        }
        break;

    case UNKNOWN_ANALYSIS:
        return false;
    }
    return false;
}


} // namespace

/*
 * Local Variables:
 * mode: c
 * c-basic-offset: 4
 * End:
 */