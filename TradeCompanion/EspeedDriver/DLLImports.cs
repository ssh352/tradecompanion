using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace EspeedDriver
{
    class DLLImports
    {
        //CALLBACK FUNCTIONS
        private  delegate void SystemCallback(int cmd, ref DLLImports.CFETI_CMD_STATUS cmdStatus, IntPtr cmdData, IntPtr CFETI_UD);
        private  delegate void ConnectCallback(int cmd, ref DLLImports.CFETI_CMD_STATUS cmdStatus, IntPtr cmdData, IntPtr CFETI_UD);

        
        //DLL IMPORT
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll")]
        public static extern int CFETIOpenSession(string primary, string secondary, ref CFETI_IDENTIFICATION_DESC oIdent);
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll")]
        public static extern string CFETIGetLastError();
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int CFETILogin(string userName, string userPassword, int systemPreferences, SystemCallback systemCallback, Object userData);
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll")]
        public static extern string CFETIVersion();
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll")]
        public static extern int CFETISelectConnectionMode(string szConnectionMode);
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll")]
        public static extern void CFETIMessageLoop();
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll")]
        public static extern int CFETILogout(String sessId, uint systemPreferences);
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll")]
        public static extern void CFETICloseSession();
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll")]
        public static extern int CFETIConnect(String sessId, String userPassword, uint tradeSys, uint tradingSysPreferences, ConnectCallback tradingSysCallback, Object userData, ref CFETI_TRADE_SETTINGS_DESC pTradeSettings);
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll")]
        public static extern int CFETIDisconnect(string sessId, uint trdSysSessId, uint tradingSysPreferences);
        [DllImport(@"C:\WINDOWS\system32\libESPD.dll")]
        public static extern int CFETIPostMessage(string sessId, uint trdSysSessId, uint cmd, IntPtr cmdData, uint cmdPreferences);


        //STRUCTURES
        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_IDENTIFICATION_DESC
        {
            public String Company;
            public String Application;
            public String Version;
            public String Platform;
            public String OperatingSystem;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct CFETI_CMD_STATUS
        {
            [MarshalAs(UnmanagedType.I4)]
            public int cmdStatus;
            [MarshalAs(UnmanagedType.LPStr)]
            public String statusText;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct CFETI_LOGIN_INFO
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string sessionId;
            public IntPtr ts;
            [MarshalAs(UnmanagedType.LPStr)]
            public string szConnectionMode;
            [MarshalAs(UnmanagedType.LPStr)]
            public string szUserId;         /**eSpeed user identity */

            //[MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.LPStruct)]
            //public CFETI_TRADING_SYS_DESC[] ts;            
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct CFETI_TRADING_SYS_DESC
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint tsId;
            [MarshalAs(UnmanagedType.LPStr)]
            public String tsDescription;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct CFETI_TRADE_SETTINGS_DESC
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint retainRule;
            [MarshalAs(UnmanagedType.U4)]
            public uint priorityFollowRule;
        }
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 2)]
        public struct CFETI_CONNECT_INFO
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint sessionId;
            [MarshalAs(UnmanagedType.Struct)]
            public CFETI_TRADING_SYS_DESC tradingSystem;
            [MarshalAs(UnmanagedType.U4)]
            public uint tradeFlags;
            [MarshalAs(UnmanagedType.U4)]
            public uint tradeFlags2;
            [MarshalAs(UnmanagedType.Struct)]
            public CFETI_TRADE_SETTINGS_DESC tradeSettings;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_INSTRUMENT_DATA_DESC
        {
            public String instName;
            public uint tsId;
            public ushort numFields;
            public IntPtr fieldTable;
            //[MarshalAs(UnmanagedType.Struct)]
            //public CFETI_FIELD_DESC fieldTable;

        }
        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_PI_BENEFIT_DESC
        {
            public double straight;     /**< The amount of PI Benefit */
            public double held;         /**< The amount of PI Held Benefit */
            public double betterFilled; /**< The amount of Better Filled Benefit */
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_FIELD_DESC
        {
            public ushort fieldId;
            public char fieldType;
            public CFETI_FIELD_VALUE fieldValue;
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_FIELD_VALUE
        {
            public char int8;
            public char Cbyte;
            public short int16;
            public ushort uint16;
            public int int32;
            public uint uint32;
            public double Cdecimal;
            public Int64 dateTime;
            public struct buffer
            {
                public ushort len;
                public String bp;
            }
            public struct bytestream
            {
                public ushort len;
                public ushort fields;
                public String bp;
            }
            public String Cstring;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CFETI_ORDER_DESC
        {
            public String tradeInstrument;
            public double price;
            public double size;
            //public char indicator;
            public byte indicator;
            public uint preferences;          /**< 32 bit mask for order preferences */
            public uint preferences2;		  /**< 32 bit mask for additional order preferences */
            public String tradeId;
            public double tradeSize;
            public /*Int64*/ int tradeTime;
            //public char tradeSide;
            public byte tradeSide;
            public double tradePrice;
            public /*Int64*/ int tradeSettlement;
            public uint settlementDate; /**< CCYYMMDD settlement date */
            public String tradeReference;		   /**< Trade reference common to all legs of a trade */
            public ushort tradeConfirmOperation; /**< Enumerated trade confirm operation */
            public uint legId;				   /**< Trade leg number for trade confirms  consisting of multiple legs */
            public uint legCount;			   /**< The number of legs in this trade */
            public uint recordVersion;		   /**< Trade confirmation version number */
            public uint id;
            public uint subsystem;
            public /*Object*/ IntPtr userData;
            public ushort userDataSize;
            public /*Object*/ IntPtr appUserData;
            public ushort appUserDataSize;
            public String userName;
            public String shortCode;
            public double toPrice; /**< Deprecated. Replaced by altPrice1 */
            public double altPrice1;
            public double altPrice2;
            public uint requestId;
            public /*Int64*/ int tradeRepoEndDate; /**< Deprecated. Replaced by endDate */
            public /*Int64*/ int endDate;
            public uint instrumentIdType;
            public String instrumentId;
            public String tradeComments;
            public uint tradeInfoType;
            public String tradeInfo;
            public uint settlementType;
            public uint orderInfoType;
            public /*Object*/ IntPtr orderInfo;
            public String counterpartyName;
            public uint counterpartyID;
            public String contactName;
            public String contactTelephoneNumber;
            public uint rejectionId;
            public uint tsId;
            public String settlementMethod;
            public double brokerage;
            public /*Int64*/ int paymentDate;
            public uint instProperties;
            public uint tradeProperties;
            public uint priceImprovement;
            public uint checkoutPermissions;
            public String reserved3;        /**< Reserved for future feature */
            public uint basketId;
            public uint basketActions;
            public String requestorId;
            public String originatorId;
            //public CFETI_INSTRUMENT_DATA_DESC instrumentData;
            public IntPtr instrumentData;
            public String clearerTradeId;
            //public CFETI_PI_BENEFIT_DESC pPIBenefit; /**< PI Benefit Data (trade confirms only) */
            public IntPtr pPIBenefit; /**< PI Benefit Data (trade confirms only) */
            public /*Int64*/ int creationTime; /**< Order creation time (responses & notifications only) */
            public String allocationInfo; /**< User specified allocation instructions (free text) */
            public uint dealStructure; /**< Enumerated deal structure */
            public uint tradeType; /**< Enumerated trade type */
            public uint pricingMethod; /**< Enumerated pricing method */
            public double executionPrice; /**< Execution price */
            public uint timeOffset; /**< No. of minutes for which order is valid when pref is GOOD_UNTIL_TIME */
            //public CFETI_TRADE_SETTINGS_DESC tradeSettings; /**< Trade settings */
            public IntPtr tradeSettings; /**< Trade settings */
            public double assetSwapLevel; /**< The corresponding asset swap level at the time of the trade */
            public double reserveMinSize; /**< Min clip size for reserve order */
            public double reserveMaxSize; /**< Max clip size for reserve order */
            public double reserveInitialSize; /**< Initial clip size for reserve order */
            public double yield; /**< Yield corresponding to screen price CFETI_ORDER_DESC::price (where available in trade confirmations) */
            public String bicCode; /**< BIC code (where available in trade confirmations) */
            public String contraTradeReference; /**< Trade indentifier (common to opposing trade confirmations) */
            public String firstPayerDtccId; /**< DTCC 8 character Id of First Payer */
            public String buyerDtccId; /**< DTCC 8 character Id of Buyer */
            public String sellerDtccId; /**< DTCC 8 character Id of Seller */
            public String brokerDtccId; /**< DTCC 8 character Id of Broker */
        }

    }
}
