﻿using System.Threading.Tasks;
using NUnit.Framework;
using FHICORC.Core.Services.DecoderServices;
using FHICORC.Core.Services.Enum;
using FHICORC.Core.Services.Interface;
using FHICORC.Tests.TestMocks;
using FHICORC.Core.Services.BusinessRules;
using FHICORC.Core.Services;
using FHICORC.Configuration;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;
using FHICORC.Tests.Factories;

namespace FHICORC.Tests.ServiceTests
{
    public class HcertTokenProcessorServiceTest
    {
        private readonly HcertTokenProcessorService service;

        public HcertTokenProcessorServiceTest()
        {
            IoCContainer.RegisterInterface<IRestClient, MockRestClient>();
            IoCContainer.RegisterInterface<IStatusBarService, MockStatusBarService>();
            service = new HcertTokenProcessorService(
                new MockCertificationService(),
                new MockDateTimeService(),
                new RuleSelectorService(
                        new MockDateTimeService(),
                        new MockBusinessRulesService(),
                        IoCContainer.Resolve<IDigitalGreenValueSetTranslatorFactory>()
                    ),
                new RuleVerifierService(new MockPreferencesService()),
                new MockPreferencesService(),
                IoCContainer.Resolve<IDigitalGreenValueSetTranslatorFactory>(),
                new MockCodingService(),
                new MockConnectivityService());
        }

        [Test]
        public async Task TestDecodeDCC_CanDecode_EC256()
        {
            //This test does not validate the signature, it just test the decoding part
            string prefixCompressedCose =
                "HC1:NCFOXN%TS3DH3ZSUZK+.V0ETD%65NL-AH-R6IOO6+IKAUE058WA7V36/9AT4V22F/8X*G3M9JUPY0BX/KR96R/S09T./0LWTKD33236J3TA3M*4VV2 73-E3GG396B-43O058YIB73A*G3W19UEBY5:PI0EGSP4*2DN43U*0CEBQ/GXQFY73CIBC:G 7376BXBJBAJ UNFMJCRN0H3PQN*E33H3OA70M3FMJIJN523.K5QZ4A+2XEN QT QTHC31M3+E32R44$28A9H0D3ZCL4JMYAZ+S-A5$XKX6T2YC 35H/ITX8GL2-LH/CJTK96L6SR9MU9RFGJA6Q3QR$P2OIC0JVLA8J3ET3:H3A+2+33U SAAUOT3TPTO4UBZIC0JKQTL*QDKBO.AI9BVYTOCFOPS4IJCOT0$89NT2V457U8+9W2KQ-7LF9-DF07U$B97JJ1D7WKP/HLIJLRKF1MFHJP7NVDEBU1J*Z222E.GJS57J5JAKA1UM %Q9D54*AHRC4VV*UI+ALZN6ZYVAT3GLM+5SH3U64B$ H%0HBHE+BOT1QHY923VAI7 PTV*C0 G2WIKTG9UM+JVY9SKS020FE/G";
            var result = await service.DecodeDCCPassportTokenToModel(prefixCompressedCose);
            //because this token is expired
            Assert.AreNotEqual(result.ValidationResult, TokenValidateResult.Invalid);
        }

        [Test]
        public async Task TestDecodeDCC_CanDecode_RSA2048()
        {
            //This test does not validate the signature, it just test the decoding part
            string prefixCompressedCose =
                "HC1:NCFG70HF0/3WUWGVLKE99.9RBVE4FTGCDH479CK$603XK2F3I7SF612F3I$RA61/IC6TAY50.FK6ZK7:EDOLFVC*70B$D% D3IA4W5646946846.966KCN9E%961A6DL6FA7D46XJCCWENF6OF63W5KF60A6WJCT3ETB8WJC0FDTA6AIA%G7X+AQB9746IG77TA$96T476:6/Q6M*8CR63Y8R46WX8F46VL6/G8SF6DR64S8+96QK4.JCP9EJY8L/5M/5546.96VF6%JC+QEEK3ZED+EDKWE3EFX3ET34X C:VDS7DM347%EKWEMED3KC.SC4KCD3DX47B46IL6646I*6..DX%DLPCG/DE$EIZAITA2IA.HA+YAU09HY8 NAE+9GY8ZJCH/DTZ9 QE5$C .CJECQW5HXO*WOZED93DXKEI3DAWEG09DH8$B9+S9 JC4/D3192KCQEDTVD$PC5$CUZC $5Z$5JPCT3E5JDOA7Q478465W58*6V50R*4SWCB4V5:TQ17.X6/O4N-VBLDT7HEKK1KD:W9Q1K4BJ2+IJ.J1Z8VTQ*DS AE2-3M6C/K4V UJ8MQ+64D7K7NA*UT250W9SN3YJA/JOFSMR4E6HI*5G0MG%U5.%0B6M+X5H*R:CVB0VR/MBWD5F8+-K*JH7YKK67TJD5$NO4DLLCX0W -M0I90-SMJPUUK+PA00ADLED*L1DF+NT6+18$B$BL:67.-E-+N52RI$U0FD.72XP7:RAM36$ 31$PHSJOV7WAT*UCYKO1LJ3+RNWSU0H0UPT:L1BR5R1 -HC-HTW6$XRC-28U7QZHO8T+%Q-I7BISNT1:8U2FUB8J$$4%Z1LX3$ DF$CL6K+USJNHWSV1EC3OBAJMHOT8NKXMPEWG61U";
            var result = await service.DecodeDCCPassportTokenToModel(prefixCompressedCose);
            //because this token is expired
            Assert.AreNotEqual(result.ValidationResult, TokenValidateResult.Invalid);
        }

        [Test]
        public async Task TestDecodeDCC_CanDecode_RSA3072()
        {
            //This test does not validate the signature, it just test the decoding part
            string prefixCompressedCose =
                "HC1:NCF*90*C0/3WUWGVLK6796/9R5M5/GWMBH479CKM603XK2F3O8J0.42F3O%I+-4/IC6TAY50.FK6ZK7:EDOLFVC*70B$D% D3IA4W5646946846.966KCN9E%961A69L6QW6B46XJCCWENF6OF63W5KF60A6WJCT3ETB8WJC0FDU56:KEPH7M/ESDD746IG77TA$96T476:6/Q6M*8CR63Y8R46WX8F46VL6/G8SF6DR64S8+96D7AQ$D.UDRYA 96NF6L/5SW6Y57+EDB.D+Y9V09HM9HC8 QE*KE0ECKQEPD09WEQDD+Q6TW6FA7C46TPCBEC8ZKW.CNWE.Y92OAGY82+8UB8-R7/0A1OA1C9K09UIAW.CE$E7%E7WE KEVKER EB39W4N*6K3/D5$CMPCG/DA8DBB85IAAY8WY8I3DA8D0EC*KE: CZ CO/EZKEZ96446C56GVC*JC1A6NA73W5KF6TF6FBBF9GJ/98:FGYB25F$4VPO9$GO7ECN:S5T3TDHU*0ZLB5CR WLYXPN:BHRSAUEJP7:6F3D2MD80QGSQ7*F8TX1OVMW7AIJ1RG4DOLJ 983U9NR+XG.A1L8CPXHB1AD8DPU3OY94WAX.B.UD9ZSPIG7TV2NMFKKE9L:+H5XQN60ULCD.9CK9:5P-+SWCJ Q6+JCVMT:2IK2HW4884UAJ1:HA+FN /CJ%TI-O1PR.IH-R6GJ5EDW7UTAIOC5RO4JZT0Y1G/W57M49JUX178X010T$B1*OG1.N/9R5*A L9:S8JR2WU4:72U7K8SI*AAS1JNLJHZUWSNB3UC/MM%CBDH$N88UM0.VQPBLML6BKE9F0S7FXQJADXOED/H%.5+:7K%5ZBK$HUH/LX074%9+RMDRDH7Q:G3E4J$LK+6I5/IJ+0U58RSLSBW.AKVZT ANOLQZW720F-%4WZV*SOHX52J0JM3F3GPFPHZKSAB6KKGV9$RJ00G:O08Z4GP3FGEU$OCOSE3HY VD05.FK 7L893VZH0RG*Q36YF7B25AR650M+8G6B$6CLOE+EFV/OE:0O*JP26X48H1";
            var result = await service.DecodeDCCPassportTokenToModel(prefixCompressedCose);
            //because this token is expired
            Assert.AreNotEqual(result.ValidationResult, TokenValidateResult.Invalid);
        }

        [Test]
        public async Task TestDecodeDCC_Faulty_COSE()
        {
            string prefixCompressedCose =
                "HC1:NCFC:MQ.NAP2H432EEY.G61FXR4.N2IMJ1*GK7US4F47VYVJ+7TF:8YLE8S4V-80GF4+5$XSW27N*J6D2S9IK*D/G9RP2E$SIIO6MHANJBV4HXK7FAS+86T5.%VIWB+KM6VM8DFG:NDN7VEQXXNBIU%JK2SH.-A%5E*LFK$QPR5:1O$56ESL+8W:WCM33RET+.L0UC TMFCB77IO0IL4HS6N:F79-1.ZT.QGM5AUC1+I3RMRBPBSFFWWA+S9S%5IZS/731QABSR5NKF2NO62Y+6 $HWZEN/RP 4 EB29UV0Z0IRKQ6TB-W28VAG2JCMHCCA9ALK4RHZHDCO-%1GF9M0LC5GU3P:40510H*9S:M%0K3QLIF57+I0*UZMAV 2TJ761OY RF10IMKE06Z$Q3V960IQXVM+9*UR9WH$DC18BH0GLBJC$4Z-FVX1SCVD-Q -3MTGC8AP1QO:6GXECZBV*1%78BMKEX2XNI7%6QGO2X8DGB8DETFM0JQA/2B+5YMBX$S2/52X0E-O/8RR*8PW7T:TYYRLWFZ62KTVDJTLTQ3BR$4E65NL-5+UP%8M14AS0C88NM%AQZFF+FCGWM3U42963UC27$Q8Y7S3VH19P$BO%*1R4K5.UT1";


            var result = await service.DecodeDCCPassportTokenToModel(prefixCompressedCose);
            Assert.AreEqual(result.ValidationResult, TokenValidateResult.Invalid);
        }

        [Test]
        public async Task TestDecodeDCC_Faulty_CBOR()
        {
            string prefixCompressedCose =
                "HC1:NCFY/LEJMBK2U13P7E/%OFQ80441MDQ7CS*N3-F6YNYJELP2QSD:S5WGKL-O9Y32UFDR90I8B5BP2O9MHJFDNH1S-17O20B2USN%-1VWHVXIOOGBTS/VLNO8VCF+6R2NOXA3VMSF0GG8KV%NRUA*XBP*VMAN$7Q007%TD6+7YQ2+L07TDT7EW-CT%G$$LW%JSEO3UO7+U2KSTXALORM9AA80GF55OO$WIO$S17LB36:3NF:02G69*MDELMX2+3BZ*QD KYDPDIQI6WTOJ5/B7Q3JK385F:UD0XA$-2CTJ8$ODS3LOQCW7ODPF6FM 7+*0B$1LNLY.0F/TKEN:ANX*8RBSI+V49EX$DR%2PZCHL20EA98P2P6Y0R$YC/90RZ0WLO0LFP6CORJU7HP MQJ53LRZ4IK4Q4V09MCZYB/NMG7JE:IA20%93$2F.8RLVMS$KWEE:HC-GJI.IY.MIF47G49+UBWAVQHFWR*QNXRLDYAN-VZLHMNDH57XZNZPNEET-UT7MUN.DV*NT02+5GO6ONAWGTLU4G6A6PW7P7V0WU/PVW.P2DW:/EJWVC-M7SCM5OM2UJ1S*HEP0403MAGWMIED8UD:LPCQT+6S.M";

            var result = await service.DecodeDCCPassportTokenToModel(prefixCompressedCose);
            Assert.AreEqual(result.ValidationResult, TokenValidateResult.Invalid);
        }

        [Test]
        public async Task TestDecodeDCC_KidHeaderInUnProtectedHeader()
        {
            string prefixCompressedCose =
                "HC1:NCFOXNRTS3DH3ZSUZK+.V0ETD%6TNLAI6LR5OGINCJMKB:X9RFCR/GKQCAQCCV4*XUA2PSGH.+HIMIBRU SITK292W7*RBT1ON1XVHWVHE 9HOP+MMBT16Y5TU1AT1SU96IAXPMMCGCNNG.8 .GYE9/MVEK0WLI+J53J9OUUMK9WLIK*L5R1ZP3YXL.T1 R1VW5G H1R5GQ3GYBKUBFX9KS5W0SRZJ3T9NYJQP59$H5NI5K1*TB3:U-1VVS1UU15%HVLI7VHAZJ7UGBL04U2YO9XO9*E6FTIPPAAMI PQVW5/O16%HAT1Z%P WUQRENS431TN$IK.G47HB%0WT072HFHN/SNP.0FVV/SN7Y431TCRVH/M$%2DU2O3J$NNP5SLAFG.CILFFDA6LFCD9KWNHPA%8L+5I9JAX.B-QFDIAS.C4-9NKE$JDVPLW1KD0K8KES/F-1JF.K+W002CT+C-Z1+168+F HTH/L5IQ1PML/3I2T5N6P U2R4$V7WJBFCRK8SH-M508%I4ANV%72M8PV-7 %4L BD$BL-DDIV$UP4D8GZS9RCS1U81A6Y0-XP-MG";

            var result = await service.DecodeDCCPassportTokenToModel(prefixCompressedCose);
            //because this token is expired
            Assert.AreNotEqual(result.ValidationResult, TokenValidateResult.Invalid);
        }

        [Test]
        public async Task TestDecodeDCC_Unknown_Prefix()
        {
            string prefixCompressedCose =
                "HC2:NCFC:M3B6+J2 53WGEY-A9S5Q34P466EDQ*NVTFI1SG%RRDIL8D6KO9BI0-EN0O/:BUTNB7FI1S512:-KC4UFJ94KGZ$HDMLONGYH8%T2B%J*P7D346K7NJLDSIPBVD5GJEU 9TI2IF5S$PV7II45H/PB9:1VHB.0H9XSI4TA:0I9M/0RTQB+$69BB+%0P%2G*P$JAJXP2-Q/PRAHK-N1 8JN/9BZD/IJ*D4C.8/KBPV3-QE6.C-QRPX8+03RXTQ94L+LHOHIS8MP51VF%EQA6I0MGX4EU41-2WLK1199 SO$P4/HI$ 9AG9HLB$HMYHKJYE-82 R5B8GP7ESMN%/B HRA73J67DXH.0BP6FO/BLBMJI8CIE4A55K5-$GQYQLM1961.T7%P4TZG:B9A9NACD* 35E3S30M8AR31V/P8T2CJPM.CH9QZO4L39N6WCE6-:03ERAEWXU8K:RLZCXK0:LHT$A+59F*TM90H0Q$CPB6I3AG6/HWDNSZE8/RO741DFA55Z%5UGJQ8N47F0ORS0Q8CR3P7J:0$3G-C9Q0KQ04YM6N9WJYSC:R%4K5/FOY6L:B$5GN2LD-L:1O 20WFWYUNUWN9OST9W:IHK.P7$VZYIQ90GD790";

            var result = await service.DecodeDCCPassportTokenToModel(prefixCompressedCose);
            Assert.AreEqual(result.ValidationResult, TokenValidateResult.Invalid);
        }

        [Test]
        public async Task TestDecodeDCC_FaultyBase45()
        {
            string prefixCompressedCose =
                "HC1:NCFC:MC9Q.P2 53QMU+7JLHI.12U6BS/EV-RH%FY7S3HJS-T7PVDTGUAH00S-AS/6S 6FU+J+5DNFBFN0MV38+O2$Q*R94KI8OKNYU/321O1KJT*JLO9ADCHKHLF.JC-E VU*$UXPUK/E*SUQ.R1%0QT14CB8SB$-LN:NMCIKGBA/V/+VQ2VDY48VMI2RMSOYX4PLJ5H9YMR0H7S.RNRN FQ IFZ.8M:EVA1EN84XCD$5GDARF5Z%H/F3%HP+K8R7CDEP6VLUMK20JBZ281OCAD:09/C80/BH519XPA9T2IIQZGWEECP4-GJRY23$MAF4WR86AU+TJ9LM060+DOQWC%%FQ:CC+CEES8G65*DHERUUNFICFVGO S/PAKF7-$G/0RLM1961.T7487F-GZLH VNACD$%3H8I530SH8LOJC4EUUKC0A:*6/AWH:QP*6YRUGOHD:7G9P0GVSY47%HPL3GEAMV8DKHK.2N7H 2B%ZAD BDIGD 71G6*-0LT5SAGJA33QFP7G%Q7OBUL*53:V5YEE1F%DVD2F .16Y5DV7KN7-%BQ4CMTUNHV8-KN8UP8T/QIWM6JOM8LV1VT.JU5L1L08BXF/$UWKO3H6I3B0:AUI76 T740=====";

            var result = await service.DecodeDCCPassportTokenToModel(prefixCompressedCose);
            Assert.AreEqual(result.ValidationResult, TokenValidateResult.Invalid);
        }

        [Test]
        public async Task TestDecodeDCC_FaultyCompressor()
        {
            string prefixCompressedCose =
                "HC1:C/A-H96N11UV/4UC-NIMUZT3V6IS TN0ULQCZO8ZXITMV85GO8EG JL2L51S*D56XJWAV/ NO 5OQU *NG1S/.F+5GE:RU5O1EU$%7T.F%BFVV70GAYUH 583JFJOT*HUTT03M0G F*C4YTLL$24KQ8$AJKLBVQZ 1-7TE1GTU44IVF%RI5G+P2HTQI4F4TA63WHKLYF00O3DMDCM7-P24X0GFEPGUCT8G5HKEE3J9%G9SCOYGU8GUN-S$KK6OFP80H56%889T1T-G%BRM7DQ68P00 HE7O7.W3YKIK10XEPL1PEQDF-BWBB%-9W4B7W91*J$IA7Z6ND9XQ4M09NT1PC43/RWG5C.2TSB:66UG3WX8.0I RPGWE2Z9+*GH08%0R$-2S838 A2UTK45IB9+B7 9SFOKXDI3Y9+NF:$4YV6*PPEYR0-45-P5OR9.NKVK XTFWVV5E IMLSKRG6TZUX/U%D78HVTZJX11J:AVKDTVULAW0PSP9W%*I6MDRIR+44K5GT*G0CHMDHS7B5GBIHAH%3X*A0:4ZJGIHH1 GIOR+SIX5VD0PZAUH:3 EK P8.T6LM4RFL2ANMWM9CU$UE8+IC54OS01+HYS1VFE:237Q2Z:NO:MU2";

            var result = await service.DecodeDCCPassportTokenToModel(prefixCompressedCose);
            Assert.AreEqual(result.ValidationResult, TokenValidateResult.Invalid);
        }

        [Test]
        public async Task TestDecodeDCC_NoopCompressor()
        {
            string prefixCompressedCose =
                "HC1:RRQT 9GO0+$F%47N23.G2O609CKIA03XK4F35KC:CE2F3EFG2LU/IC6TAY50.FKP1LLWEYWEYPCZ$E7ZKPEDMHG7ECMUDQF60R6BB87M8BL6-A6HPCTB8IECDJCD7AW.CXJD7%E7WE KEVKEZ$EI3DA8D0EC*KE: CZ CGVC*JC1A6NA73W5KF6TF6$PC1ECFNGGPCEVCD8FI-AIPCZEDIEC EDM-DKPCG/DZKE/34QEDA/DOCCQF6$PCEEC5JD%96IA7B46646WX6JECFWEB1A-:8$966469L6OF6WX6F$DP9EJY8L/5M/5546.96VF6YPC4$C+70AVCPQEX47B46IL6646I*6UPCZ$ETB8RPC24EQ DPF6BW5E%6Z*83W5746JPCIEC6JD846Y968464W5Z57HWE/TEEOL2ECY$D9Q31ECOPC..DBWE-3EB441$CKWEDZCQ-A1$C..D734FM6K/EN9E%961A69L6QW6B46GPC8%E% D3IA4W5646946846.96SPC3$C.UDRYA 96NF6L/5SW6Z57LQE+CEJPC+EDQDD+Q6TW6FA7C46IPC34E/IC3UA*VDFWECM8KF6 590G6A*8746C562VCCWENF6OF63W56L6*96ZPC24EOD0P/EH.EY$5Y$5XPCZ CJAG8VCOPCPVC..49A61TAOF6LA7WW68463:6QF6A46UF6+Q6RF6//6SF6H%6NF6SF6646.Q627B5JQ0H3/.HJ2S%3U7SKVGH835.R7JL24/N$5TJZ2V32NCUPSOAWNNJSY$23RS+5TT4M862*O6IPDVZ8L1P0FQR6KHFC9Z7VDF";
            var result = await service.DecodeDCCPassportTokenToModel(prefixCompressedCose);
            Assert.AreEqual(result.ValidationResult, TokenValidateResult.Invalid);
        }

        [Test]
        public async Task TestDecodeSHC_ValidToken_ReturnsValid()
        {
            var result = await service.DecodeSHCPassportTokenToModel(SmartHealthCardFactory.CreateEncodedJwsToken());

            Assert.AreEqual(TokenValidateResult.Valid, result.ValidationResult);
        }

        [Test]
        public async Task TestDecodeSHC_EmptyToken_ReturnsInvalid()
        {
            var result = await service.DecodeSHCPassportTokenToModel("shc:/");

            Assert.AreEqual(TokenValidateResult.Invalid, result.ValidationResult);
        }

        [Test]
        public async Task TestDecodeSHC_ThreeInvalidParts_ReturnsInvalid()
        {
            // 01 = .
            var result = await service.DecodeSHCPassportTokenToModel("shc:/123545645234" +
                "01" +
                "341654231213" +
                "01" +
                "3121232134322");

            Assert.AreEqual(TokenValidateResult.Invalid, result.ValidationResult);
        }

        [Test]
        public async Task TestDecodeSHC_TwoParts_ReturnsInvalid()
        {
            // 01 = .
            var result = await service.DecodeSHCPassportTokenToModel("shc:/123545645234" +
                "341654231213" +
                "01" +
                "3121232134322");

            Assert.AreEqual(TokenValidateResult.Invalid, result.ValidationResult);
        }

        [Test]
        public async Task TestDecodeSHC_FourParts_ReturnsInvalid()
        {
            // 01 = .
            var result = await service.DecodeSHCPassportTokenToModel("shc:/123545645234" +
                "01" +
                "341654231213" +
                "01" +
                "3423121303" +
                "01" +
                "3121232134322");

            Assert.AreEqual(TokenValidateResult.Invalid, result.ValidationResult);
        }
    }
}