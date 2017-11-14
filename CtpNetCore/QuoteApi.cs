using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace PureCode.CtpCSharp
{
    public class QuoteApi
    {
        private AssembyLoader loader;
        IntPtr _handle = IntPtr.Zero, _api = IntPtr.Zero, _spi = IntPtr.Zero;
        delegate IntPtr Create();
        delegate IntPtr DelegateRegisterSpi(IntPtr api, IntPtr pSpi);
        public QuoteApi(string pFile)
        {
            loader = new AssembyLoader(pFile);
            Directory.CreateDirectory("log");

            _api = (loader.Invoke("CreateApi", typeof(Create)) as Create)();
            _spi = (loader.Invoke("CreateSpi", typeof(Create)) as Create)();
            (loader.Invoke("RegisterSpi", typeof(DelegateRegisterSpi)) as DelegateRegisterSpi)(_api, _spi);
        }


        #region 声明REQ函数类型
        public delegate IntPtr DelegateRelease(IntPtr api);
        public delegate IntPtr DelegateInit(IntPtr api);
        public delegate IntPtr DelegateJoin(IntPtr api);
        public delegate IntPtr DelegateGetTradingDay(IntPtr api);
        public delegate IntPtr DelegateRegisterFront(IntPtr api, string pszFrontAddress);
        public delegate IntPtr DelegateRegisterNameServer(IntPtr api, string pszNsAddress);
        public delegate IntPtr DelegateRegisterFensUserInfo(IntPtr api, CThostFtdcFensUserInfoField pFensUserInfo);
        public delegate IntPtr DelegateSubscribeMarketData(IntPtr api, IntPtr pInstruments, int pCount);
        public delegate IntPtr DelegateUnSubscribeMarketData(IntPtr api, IntPtr pInstruments, int pCount);
        public delegate IntPtr DelegateSubscribeForQuoteRsp(IntPtr api, IntPtr pInstruments, int pCount);
        public delegate IntPtr DelegateUnSubscribeForQuoteRsp(IntPtr api, IntPtr pInstruments, int pCount);
        public delegate IntPtr DelegateReqUserLogin(IntPtr api, CThostFtdcReqUserLoginField pReqUserLoginField, int nRequestID);
        public delegate IntPtr DelegateReqUserLogout(IntPtr api, CThostFtdcUserLogoutField pUserLogout, int nRequestID);

        #endregion

        #region REQ函数

        private int nRequestID = 0;

        public delegate IntPtr DelegateVersion();
        public IntPtr Version()
        {
            return (loader.Invoke("Version", typeof(DelegateVersion)) as DelegateVersion)();
        }

        public IntPtr Release()
        {
            (loader.Invoke("RegisterSpi", typeof(DelegateRegisterSpi)) as DelegateRegisterSpi)(_api, IntPtr.Zero);
            return (loader.Invoke("Release", typeof(DelegateRelease)) as DelegateRelease)(_api);
        }

        public IntPtr Init()
        {
            return (loader.Invoke("Init", typeof(DelegateInit)) as DelegateInit)(_api);
        }

        public IntPtr Join()
        {
            return (loader.Invoke("Join", typeof(DelegateJoin)) as DelegateJoin)(_api);
        }

        public IntPtr GetTradingDay()
        {
            return (loader.Invoke("GetTradingDay", typeof(DelegateGetTradingDay)) as DelegateGetTradingDay)(_api);
        }

        public IntPtr RegisterFront(string pszFrontAddress)
        {
            return (loader.Invoke("RegisterFront", typeof(DelegateRegisterFront)) as DelegateRegisterFront)(_api, pszFrontAddress);
        }

        public IntPtr RegisterNameServer(string pszNsAddress)
        {
            return (loader.Invoke("RegisterNameServer", typeof(DelegateRegisterNameServer)) as DelegateRegisterNameServer)(_api, pszNsAddress);
        }

        public IntPtr RegisterFensUserInfo(string BrokerID = "", string UserID = "", TThostFtdcLoginModeType LoginMode = TThostFtdcLoginModeType.THOST_FTDC_LM_Trade)
        {
            CThostFtdcFensUserInfoField struc = new CThostFtdcFensUserInfoField
            {
                BrokerID = BrokerID,
                UserID = UserID,
                LoginMode = LoginMode,
            };
            return (loader.Invoke("RegisterFensUserInfo", typeof(DelegateRegisterFensUserInfo)) as DelegateRegisterFensUserInfo)(_api, struc);
        }

        public IntPtr SubscribeMarketData(IntPtr pInstruments, int pCount)
        {
            return (loader.Invoke("SubscribeMarketData", typeof(DelegateSubscribeMarketData)) as DelegateSubscribeMarketData)(_api, pInstruments, pCount);
        }

        public IntPtr UnSubscribeMarketData(IntPtr pInstruments, int pCount)
        {
            return (loader.Invoke("UnSubscribeMarketData", typeof(DelegateUnSubscribeMarketData)) as DelegateUnSubscribeMarketData)(_api, pInstruments, pCount);
        }

        public IntPtr SubscribeForQuoteRsp(IntPtr pInstruments, int pCount)
        {
            return (loader.Invoke("SubscribeForQuoteRsp", typeof(DelegateSubscribeForQuoteRsp)) as DelegateSubscribeForQuoteRsp)(_api, pInstruments, pCount);
        }

        public IntPtr UnSubscribeForQuoteRsp(IntPtr pInstruments, int pCount)
        {
            return (loader.Invoke("UnSubscribeForQuoteRsp", typeof(DelegateUnSubscribeForQuoteRsp)) as DelegateUnSubscribeForQuoteRsp)(_api, pInstruments, pCount);
        }

        public IntPtr ReqUserLogin(string TradingDay = "", string BrokerID = "", string UserID = "", string Password = "", string UserProductInfo = "", string InterfaceProductInfo = "", string ProtocolInfo = "", string MacAddress = "", string OneTimePassword = "", string ClientIPAddress = "", string LoginRemark = "")
        {
            CThostFtdcReqUserLoginField struc = new CThostFtdcReqUserLoginField
            {
                TradingDay = TradingDay,
                BrokerID = BrokerID,
                UserID = UserID,
                Password = Password,
                UserProductInfo = UserProductInfo,
                InterfaceProductInfo = InterfaceProductInfo,
                ProtocolInfo = ProtocolInfo,
                MacAddress = MacAddress,
                OneTimePassword = OneTimePassword,
                ClientIPAddress = ClientIPAddress,
                LoginRemark = LoginRemark,
            };
            return (loader.Invoke("ReqUserLogin", typeof(DelegateReqUserLogin)) as DelegateReqUserLogin)(_api, struc, this.nRequestID++);
        }

        public IntPtr ReqUserLogout(string BrokerID = "", string UserID = "")
        {
            CThostFtdcUserLogoutField struc = new CThostFtdcUserLogoutField
            {
                BrokerID = BrokerID,
                UserID = UserID,
            };
            return (loader.Invoke("ReqUserLogout", typeof(DelegateReqUserLogout)) as DelegateReqUserLogout)(_api, struc, this.nRequestID++);
        }

        #endregion

        delegate void DelegateSet(IntPtr spi, Delegate func);

        public delegate void OnFrontConnectedAction();
        public void SetOnFrontConnected(OnFrontConnectedAction func) { (loader.Invoke("SetOnFrontConnected", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnFrontDisconnectedAction(int nReason);
        public void SetOnFrontDisconnected(OnFrontDisconnectedAction func) { (loader.Invoke("SetOnFrontDisconnected", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnHeartBeatWarningAction(int nTimeLapse);
        public void SetOnHeartBeatWarning(OnHeartBeatWarningAction func) { (loader.Invoke("SetOnHeartBeatWarning", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnRspUserLoginAction(ref CThostFtdcRspUserLoginField pRspUserLogin, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        public void SetOnRspUserLogin(OnRspUserLoginAction func) { (loader.Invoke("SetOnRspUserLogin", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnRspUserLogoutAction(ref CThostFtdcUserLogoutField pUserLogout, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        public void SetOnRspUserLogout(OnRspUserLogoutAction func) { (loader.Invoke("SetOnRspUserLogout", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnRspErrorAction(ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        public void SetOnRspError(OnRspErrorAction func) { (loader.Invoke("SetOnRspError", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnRspSubMarketDataAction(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        public void SetOnRspSubMarketData(OnRspSubMarketDataAction func) { (loader.Invoke("SetOnRspSubMarketData", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnRspUnSubMarketDataAction(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        public void SetOnRspUnSubMarketData(OnRspUnSubMarketDataAction func) { (loader.Invoke("SetOnRspUnSubMarketData", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnRspSubForQuoteRspAction(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        public void SetOnRspSubForQuoteRsp(OnRspSubForQuoteRspAction func) { (loader.Invoke("SetOnRspSubForQuoteRsp", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnRspUnSubForQuoteRspAction(ref CThostFtdcSpecificInstrumentField pSpecificInstrument, ref CThostFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast);
        public void SetOnRspUnSubForQuoteRsp(OnRspUnSubForQuoteRspAction func) { (loader.Invoke("SetOnRspUnSubForQuoteRsp", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnRtnDepthMarketDataAction(ref CThostFtdcDepthMarketDataField pDepthMarketData);
        public void SetOnRtnDepthMarketData(OnRtnDepthMarketDataAction func) { (loader.Invoke("SetOnRtnDepthMarketData", typeof(DelegateSet)) as DelegateSet)(_spi, func); }

        public delegate void OnRtnForQuoteRspAction(ref CThostFtdcForQuoteRspField pForQuoteRsp);
        public void SetOnRtnForQuoteRsp(OnRtnForQuoteRspAction func) { (loader.Invoke("SetOnRtnForQuoteRsp", typeof(DelegateSet)) as DelegateSet)(_spi, func); }
    }
}
