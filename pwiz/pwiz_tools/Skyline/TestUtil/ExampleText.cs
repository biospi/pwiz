﻿/*
 * Original author: Brendan MacLean <brendanx .at. u.washington.edu>,
 *                  MacCoss Lab, Department of Genome Sciences, UW
 *
 * Copyright 2010 University of Washington - Seattle, WA
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
namespace pwiz.SkylineTestUtil
{
    public static class ExampleText
    {
        public const string TEXT_FASTA_YEAST =
            ">YAL001C TFC3 SGDID:S000000001, Chr I from 151168-151099,151008-147596, reverse complement, Verified ORF, \"Largest of six subunits of the RNA polymerase III transcription initiation factor complex (TFIIIC); part of the TauB domain of TFIIIC that binds DNA at the BoxB promoter sites of tRNA and similar genes; cooperates with Tfc6p in DNA binding\"\n" +
            "MVLTIYPDELVQIVSDKIASNKGKITLNQLWDISGKYFDLSDKKVKQFVLSCVILKKDIE\n" +
            "VYCDGAITTKNVTDIIGDANHSYSVGITEDSLWTLLTGYTKKESTIGNSAFELLLEVAKS\n" +
            "GEKGINTMDLAQVTGQDPRSVTGRIKKINHLLTSSQLIYKGHVVKQLKLKKFSHDGVDSN\n" +
            "PYINIRDHLATIVEVVKRSKNGIRQIIDLKRELKFDKEKRLSKAFIAAIAWLDEKEYLKK\n" +
            "VLVVSPKNPAIKIRCVKYVKDIPDSKGSPSFEYDSNSADEDSVSDSKAAFEDEDLVEGLD\n" +
            "NFNATDLLQNQGLVMEEKEDAVKNEVLLNRFYPLQNQTYDIADKSGLKGISTMDVVNRIT\n" +
            "GKEFQRAFTKSSEYYLESVDKQKENTGGYRLFRIYDFEGKKKFFRLFTAQNFQKLTNAED\n" +
            "EISVPKGFDELGKSRTDLKTLNEDNFVALNNTVRFTTDSDGQDIFFWHGELKIPPNSKKT\n" +
            "PNKNKRKRQVKNSTNASVAGNISNPKRIKLEQHVSTAQEPKSAEDSPSSNGGTVVKGKVV\n" +
            "NFGGFSARSLRSLQRQRAILKVMNTIGGVAYLREQFYESVSKYMGSTTTLDKKTVRGDVD\n" +
            "LMVESEKLGARTEPVSGRKIIFLPTVGEDAIQRYILKEKDSKKATFTDVIHDTEIYFFDQ\n" +
            "TEKNRFHRGKKSVERIRKFQNRQKNAKIKASDDAISKKSTSVNVSDGKIKRRDKKVSAGR\n" +
            "TTVVVENTKEDKTVYHAGTKDGVQALIRAVVVTKSIKNEIMWDKITKLFPNNSLDNLKKK\n" +
            "WTARRVRMGHSGWRAYVDKWKKMLVLAIKSEKISLRDVEELDLIKLLDIWTSFDEKEIKR\n" +
            "PLFLYKNYEENRKKFTLVRDDTLTHSGNDLAMSSMIQREISSLKKTYTRKISASTKDLSK\n" +
            "SQSDDYIRTVIRSILIESPSTTRNEIEALKNVGNESIDNVIMDMAKEKQIYLHGSKLECT\n" +
            "DTLPDILENRGNYKDFGVAFQYRCKVNELLEAGNAIVINQEPSDISSWVLIDLISGELLN\n" +
            "MDVIPMVRNVRPLTYTSRRFEIRTLTPPLIIYANSQTKLNTARKSAVKVPLGKPFSRLWV\n" +
            "NGSGSIRPNIWKQVVTMVVNEIIFHPGITLSRLQSRCREVLSLHEISEICKWLLERQVLI\n" +
            "TTDFDGYWVNHNWYSIYEST*\n" +
            ">YAL002W VPS8 SGDID:S000000002, Chr I from 143709-147533, Verified ORF, \"Membrane-associated hydrophilic protein that interacts with the small GTPase, Vps21p, to facilitate soluble vacuolar protein localization; required for localization and trafficking of the CPY sorting receptor; contains a RING finger motif\"\n" +
            "MEQNGLDHDSRSSIDTTINDTQKTFLEFRSYTQLSEKLASSSSYTAPPLNEDGPKGVASA\n" +
            "VSQGSESVVSWTTLTHVYSILGAYGGPTCLYPTATYFLMGTSKGCVLIFNYNEHLQTILV\n" +
            "PTLSEDPSIHSIRSPVKSIVICSDGTHVAASYETGNICIWNLNVGYRVKPTSEPTNGMTP\n" +
            "TPALPAVLHIDDHVNKEITGLDFFGARHTALIVSDRTGKVSLYNGYRRGFWQLVYNSKKI\n" +
            "LDVNSSKEKLIRSKLSPLISREKISTNLLSVLTTTHFALILLSPHVSLMFQETVEPSVQN\n" +
            "SLVVNSSISWTQNCSRVAYSVNNKISVISISSSDFNVQSASHSPEFAESILSIQWIDQLL\n" +
            "LGVLTISHQFLVLHPQHDFKILLRLDFLIHDLMIPPNKYFVISRRSFYLLTNYSFKIGKF\n" +
            "VSWSDITLRHILKGDYLGALEFIESLLQPYCPLANLLKLDNNTEERTKQLMEPFYNLSLA\n" +
            "ALRFLIKKDNADYNRVYQLLMVVVRVLQQSSKKLDSIPSLDVFLEQGLEFFELKDNAVYF\n" +
            "EVVANIVAQGSVTSISPVLFRSIIDYYAKEENLKVIEDLIIMLNPTTLDVDLAVKLCQKY\n" +
            "NLFDLLIYIWNKIFDDYQTPVVDLIYRISNQSEKCVIFNGPQVPPETTIFDYVTYILTGR\n" +
            "QYPQNLSISPSDKCSKIQRELSAFIFSGFSIKWPSNSNHKLYICENPEEEPAFPYFHLLL\n" +
            "KSNPSRFLAMLNEVFEASLFNDDNDMVASVGEAELVSRQYVIDLLLDAMKDTGNSDNIRV\n" +
            "LVAIFIATSISKYPQFIKVSNQALDCVVNTICSSRVQGIYEISQIALESLLPYYHSRTTE\n" +
            "NFILELKEKNFNKVLFHIYKSENKYASALSLILETKDIEKEYNTDIVSITDYILKKCPPG\n" +
            "SLECGKVTEVIETNFDLLLSRIGIEKCVTIFSDFDYNLHQEILEVKNEETQQKYLDKLFS\n" +
            "TPNINNKVDKRLRNLHIELNCKYKSKREMILWLNGTVLSNAESLQILDLLNQDSNFEAAA\n" +
            "IIHERLESFNLAVRDLLSFIEQCLNEGKTNISTLLESLRRAFDDCNSAGTEKKSCWILLI\n" +
            "TFLITLYGKYPSHDERKDLCNKLLQEAFLGLVRSKSSSQKDSGGEFWEIMSSVLEHQDVI\n" +
            "LMKVQDLKQLLLNVFNTYKLERSLSELIQKIIEDSSQDLVQQYRKFLSEGWSIHTDDCEI\n" +
            "CGKKIWGAGLDPLLFLAWENVQRHQDMISVDLKTPLVIFKCHHGFHQTCLENLAQKPDEY\n" +
            "SCLICQTESNPKIV*";

        public const string TEXT_FASTA_YEAST_LIB =
            ">YAL008W FUN14 SGDID:S000000006, Chr I from 136916-137512, Verified ORF, \"Mitochondrial protein of unknown function\"\n" +
            "MTLAFNMQRLVFRNLNVGKRMFKNVPLWRFNVANKLGKPLTRSVGLGGAGIVAGGFYLMN\n" +
            "RQPSKLIFNDSLGAAVKQQGPLEPTVGNSTAITEERRNKISSHKQMFLGSLFGVVLGVTV\n" +
            "AKISILFMYVGITSMLLCEWLRYKGWIRINLKNIKSVIVLKDVDLKKLLIDGLLGTEYMG\n" +
            "FKVFFTLSFVLASLNANK*\n" +
            ">YAL010C MDM10 SGDID:S000000008, Chr I from 135667-134186, reverse complement, Verified ORF, \"Subunit of the mitochondrial sorting and assembly machinery (SAM complex); has a role in assembly of the TOM complex, which mediates protein import through the outer membrane; required for normal mitochondrial morphology and inheritance\"\n" +
            "MLPYMDQVLRAFYQSTHWSTQNSYEDITATSRTLLDFRIPSAIHLQISNKSTPNTFNSLD\n" +
            "FSTRSRINGSLSYLYSDAQQLEKFMRNSTDIPLQDATETYRQLQPNLNFSVSSANTLSSD\n" +
            "NTTVDNDKKLLHDSKFVKKSLYYGRMYYPSSDLEAMIIKRLSPQTQFMLKGVSSFKESLN\n" +
            "VLTCYFQRDSHRNLQEWIFSTSDLLCGYRVLHNFLTTPSKFNTSLYNNSSLSLGAEFWLG\n" +
            "LVSLSPGCSTTLRYYTHSTNTGRPLTLTLSWQPLFGHISSTYSAKTGTNSTFCAKYDFNL\n" +
            "YSIESNLSFGCEFWQKKHHLLETNKNNNDKLEPISDELVDINPNSRATKLLHENVPDLNS\n" +
            "AVNDIPSTLDIPVHKQKLLNDLTYAFSSSLRKIDEERSTIEKFDNKINSSIFTSVWKLST\n" +
            "SLRDKTLKLLWEGKWRGFLISAGTELVFTRGFQESLSDDEKNDNAISISATDTENGNIPV\n" +
            "FPAKFGIQFQYST*";
    }
}