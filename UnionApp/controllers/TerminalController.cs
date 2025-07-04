using Helpers;
using Helpers.controllers;
using HelpersNetFramework.classes;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using UCBioBSPCOMLib;
using UCSAPICOMLib;
using UnionApp.classes;

namespace UnionApp.controllers
{
    public static class TerminalController
    {
        public static BindingList<Device> devices = new BindingList<Device>();
        public static BindingList<Monitoring> monitorings = new BindingList<Monitoring>();

        public static bool running = false;

        public static string szTextEnrolledFIR;
        public static byte[] binaryEnrolledFIR;

        //public static WALKTHROUGH_DATA WalkThrough;

        public static readonly long nTemplateType400 = 400;
        public static readonly long nTemplateType800 = 800;
        public static readonly long nTemplateType320 = 320;
        public static readonly long nTemplateType256 = 256;

        // UCSAPI
        public static UCSAPI ucsAPI;
        public static IServerUserData serveruserData;
        public static ITerminalUserData terminalUserData;
        public static IServerAuthentication serverAuthentication;
        public static IAccessLogData accessLogData;
        public static ITerminalOption terminalOption;
        public static IAccessControlData accessControlData;

        // UCBioBSP
        public static UCBioBSP ucBioBSP;
        public static IFPData fpData;
        public static IFPImage testimg;

        //private ITemplateInfo templateInfo;
        public static IDevice device;
        public static IExtraction extraction;
        public static IFastSearch fastSearch;
        public static IMatching matching;

        public static Procedure procedure;
        public static SqlTemplates sqlTemplates;

        //Timer
        public static System.Timers.Timer timerGetDevices;

        public static void Start()
        {
            timerGetDevices = new System.Timers.Timer(1000 * 60);
            timerGetDevices.Elapsed += timerGetDevices_Elapsed;
            timerGetDevices.Start();

            timerGetDevices_Elapsed(null, null);


            ucsAPI = new UCSAPIClass();

            serveruserData = ucsAPI.ServerUserData as IServerUserData;
            terminalUserData = ucsAPI.TerminalUserData as ITerminalUserData;
            accessLogData = ucsAPI.AccessLogData as IAccessLogData;
            serverAuthentication = ucsAPI.ServerAuthentication as IServerAuthentication;
            terminalOption = ucsAPI.TerminalOption as ITerminalOption;
            accessControlData = ucsAPI.AccessControlData as IAccessControlData;

            // create UCBioBSPClass Instance
            ucBioBSP = new UCBioBSPClass();
            fpData = ucBioBSP.FPData as IFPData;
            device = ucBioBSP.Device as IDevice;
            extraction = ucBioBSP.Extraction as IExtraction;
            fastSearch = ucBioBSP.FastSearch as IFastSearch;
            matching = ucBioBSP.Matching as IMatching;

            try
            {
                ucsAPI.ServerStart(255, MainController.config.listenerPort);
                SetEvent();

                running = true;
                LogController.Information("Server started on port " + MainController.config.listenerPort, true);
            }
            catch (Exception ex)
            {
                LogController.Error("Error starting server: " + ex.Message);
            }
        }

        public static void Stop()
        {
            if (timerGetDevices != null)
            {
                timerGetDevices.Stop();
                timerGetDevices.Dispose();
                timerGetDevices = null;
            }

            ucsAPI.ServerStop();
            ucsAPI = null;
            running = false;
            LogController.Information("Server stopped", true);
        }

        public static async void timerGetDevices_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!OracleDb.IsConnected)
            {
                LogController.Error("Database not connected, Cannot get devices.");
                return;
            }

            if (!string.IsNullOrEmpty(sqlTemplates.DEVICE_GET_LIST))
            {
                var dt = await OracleDb.excuteSQLAsync(sqlTemplates.DEVICE_GET_LIST);
                var deviceList = Helper.DatatableToList<Device>(dt);

                if (deviceList.Count > 0)
                {
                    for (int i = 0; i < deviceList.Count; i++)
                    {
                        lock (devices)
                        {
                            var device = deviceList[i];
                            int idx = devices.ToList().FindIndex(d => d.Id == device.Id);

                            if (idx < 0)
                            {
                                devices.Add(device);
                            }
                            else
                            {
                                devices[idx].Name = device.Name;
                                devices[idx].CompanyPk = device.CompanyPk;
                                devices[idx].CompanyBranch = device.CompanyBranch;
                                devices[idx].AccessGroup = device.AccessGroup;
                            }
                        }
                    }
                }
                else
                {

                }
            }

        }

        private static void SetEvent()
        {
            ucsAPI.EventTerminalConnected += new _DIUCSAPIEvents_EventTerminalConnectedEventHandler(UCSCOMObj_EventTerminalConnected);
            ucsAPI.EventTerminalDisconnected += new _DIUCSAPIEvents_EventTerminalDisconnectedEventHandler(UCSCOMObj_EventTerminalDisconnected);
            ucsAPI.EventTerminalStatus += new _DIUCSAPIEvents_EventTerminalStatusEventHandler(ucsAPI_EventTerminalStatus);
            ucsAPI.EventRealTimeAccessLog += new _DIUCSAPIEvents_EventRealTimeAccessLogEventHandler(ucsAPI_EventRealTimeAccessLog);
            //ucsAPI.EventFirmwareUpgraded += new _DIUCSAPIEvents_EventFirmwareUpgradedEventHandler(ucsAPI_EventFirmwareUpgraded);
            //ucsAPI.EventFirmwareUpgrading += new _DIUCSAPIEvents_EventFirmwareUpgradingEventHandler(ucsAPI_EventFirmwareUpgrading);
            ucsAPI.EventFirmwareVersion += new _DIUCSAPIEvents_EventFirmwareVersionEventHandler(ucsAPI_EventFirmwareVersion);
            ucsAPI.EventGetUserCount += new _DIUCSAPIEvents_EventGetUserCountEventHandler(ucsAPI_EventGetUserCount);
            //ucsAPI.EventGetAccessLogCount += new _DIUCSAPIEvents_EventGetAccessLogCountEventHandler(ucsAPI_EventGetAccessLogCount);
            //ucsAPI.EventPictureLog += new _DIUCSAPIEvents_EventPictureLogEventHandler(ucsAPI_EventPictureLog);
            //ucsAPI.EventDeleteAllUser += new _DIUCSAPIEvents_EventDeleteAllUserEventHandler(ucsAPI_EventDeleteAllUser);
            //ucsAPI.EventSetAccessControlData += new _DIUCSAPIEvents_EventSetAccessControlDataEventHandler(ucsAPI_EventSetAccessControlData);

            ucsAPI.EventGetAccessLog += new _DIUCSAPIEvents_EventGetAccessLogEventHandler(ucsAPI_EventGetAccessLog);
            //ucsAPI.EventAddUser += new _DIUCSAPIEvents_EventAddUserEventHandler(ucsAPI_EventAddUser);
            //ucsAPI.EventDeleteUser += new _DIUCSAPIEvents_EventDeleteUserEventHandler(ucsAPI_EventDeleteUser);
            ucsAPI.EventGetUserInfoList += new _DIUCSAPIEvents_EventGetUserInfoListEventHandler(ucsAPI_EventGetUserInfoList);
            ucsAPI.EventGetUserData += new _DIUCSAPIEvents_EventGetUserDataEventHandler(ucsAPI_EventGetUserData);
            //ucsAPI.EventOpenDoor += new _DIUCSAPIEvents_EventOpenDoorEventHandler(ucsAPI_EventOpenDoor);
            //ucsAPI.EventUserFileUpgraded += new _DIUCSAPIEvents_EventUserFileUpgradedEventHandler(ucsAPI_EventUserFileUpgraded);
        }


        /*====================================================================================================================
         Event Handlers
         ====================================================================================================================*/
        #region        // Event Handlers
        private static void UCSCOMObj_EventTerminalConnected(int TerminalID, string TerminalIP)
        { 
            int idx = devices.ToList().FindIndex(d => d.Id == TerminalID);
            if (idx < 0)
            {
                devices.Add(new Device
                {
                    IP = TerminalIP,
                    Status = DEVICE_STATUS.CONNECTED
                });

                idx = devices.ToList().FindIndex(d => d.Id == TerminalID);
            }
            else
            {
                devices[idx].Status = DEVICE_STATUS.CONNECTED;
                devices[idx].IP = TerminalIP;
            }

            UpdateDatabase(nameof(sqlTemplates.DEVICE_UPDATE_STATUS), sqlTemplates.DEVICE_UPDATE_STATUS, devices[idx]);

            LogController.Information($"Terminal {TerminalID}[{TerminalIP}] connected", true);
        }

        private static void UCSCOMObj_EventTerminalDisconnected(int TerminalID)
        {
            int idx = devices.ToList().FindIndex(d => d.Id == TerminalID);
            if (idx >= 0)
            {
                devices[idx].Status = DEVICE_STATUS.DISCONNECTED;
            }

            UpdateDatabase(nameof(sqlTemplates.DEVICE_UPDATE_STATUS), sqlTemplates.DEVICE_UPDATE_STATUS, devices[idx]);

            LogController.Information($"Terminal {TerminalID} disconnected", true);
        }

        private static void ucsAPI_EventTerminalStatus(int ClientID, int TerminalID, int TerminalStatus, int DoorStatus, int CoverStatus)
        { 
        
        }

        private static void ucsAPI_EventRealTimeAccessLog(int TerminalID)
        {
            Monitoring monitoring = new Monitoring()
            { 
                DeviceId = TerminalID,
                UserId = accessLogData.UserID,
                Time = accessLogData.DateTime,
                AuthMode = accessLogData.AuthMode.ToString(),
                AuthType = accessLogData.AuthType.ToString(),
                AuthResult = accessLogData.AuthResult.ToString(),
                ThermalBurn = accessLogData.ThermalBurn.ToString()
            };

            if (accessLogData.PictureDataLength > 0)
            {
                try
                {
                    MemoryStream ms = null;
                    using (ms = new MemoryStream((byte[])accessLogData.PictureData))
                    {
                        monitoring.Image = Image.FromStream(ms);
                        ms.Close();
                        ms.Dispose();
                    }
                }
                catch { }
            }
            monitorings.Insert(0, monitoring);

            if (monitorings.Count > 500)
            {
                monitorings.RemoveAt(monitorings.Count - 1); // Keep the last 500 monitorings
            }

            UpdateDatabase(nameof(sqlTemplates.THR_ENTER_INSERT), sqlTemplates.THR_ENTER_INSERT, monitoring);
            UpdateDatabase(nameof(sqlTemplates.THR_TIME_TEMP_INSERT), sqlTemplates.THR_TIME_TEMP_INSERT, monitoring);
        }

        private static void ucsAPI_EventFirmwareVersion(int ClientID, int TerminalID, string Version)
        { 
        
        }

        private static void ucsAPI_EventGetUserCount(int ClientID, int TerminalID, int AdminCount, int UserCount)
        {
            
        }

        private static void ucsAPI_EventGetAccessLog(int ClientID, int TerminalID)
        { 
        
        }

        private static void ucsAPI_EventGetUserInfoList(int ClientID, int TerminalID)
        {
            
        }

        private static void ucsAPI_EventGetUserData(int ClientID, int TerminalID)
        {

        }

        #endregion


        /*====================================================================================================================
         PROCESS Handlers
         ====================================================================================================================*/

        private static async Task UpdateDatabase(string template, string sql, object data)
        {
            try
            {
                if (template == nameof(sqlTemplates.DEVICE_UPDATE_STATUS))
                {
                    Device device = data as Device;
                    sql = Helper.ReplaceText(sql, device);

                    if (device.Id > 0)
                    {
                        await OracleDb.excuteSQLCommandAsync(sql);
                    }

                    devices.ResetBindings();
                }

                else if (template == nameof(sqlTemplates.THR_ENTER_INSERT))
                {
                    Monitoring monitoring = data as Monitoring;
                    Device device = devices.ToList().Find(d => d.Id == monitoring.DeviceId);

                    sql = Helper.ReplaceText(sql, monitoring);
                    sql = Helper.ReplaceText(sql, device);

                    List<OraclePara> oracleParas = new List<OraclePara>
                    {
                        new OraclePara { value = monitoring.DeviceId   }
                        , new OraclePara {  value = monitoring.UserId   }
                        , new OraclePara { value = Helper.RemoveSpecialCharacters(monitoring.Time) }
                        , new OraclePara { value = device.CompanyPk }
                        , new OraclePara { value = device.CompanyBranch }
                        , new OraclePara { value = monitoring.AuthType }
                        , new OraclePara { value = monitoring.AuthMode }
                        , new OraclePara { value = monitoring.AuthResult }
                        , new OraclePara { value = 0 }
                        , new OraclePara { value = DateTime.Now }
                        , new OraclePara { value = "SDK-REALTIME" }
                        , new OraclePara { value = null }
                        , new OraclePara { value = monitoring.ThermalBurn }
                        , new OraclePara { value = monitoring.ImageBytes != null ? monitoring.ImageBytes : null, type = OracleDbType.Blob }
                        , new OraclePara { value = monitoring.ImageBytes != null ? monitoring.ImageBytes.Length : 0 }
                    };

                    await OracleDb.excuteSQLCommandAsync(sql, oracleParas);
                }
                else if (template == nameof(sqlTemplates.THR_TIME_TEMP_INSERT))
                {
                    Monitoring monitoring = data as Monitoring;
                    
                }
            }
            catch(Exception ex)
            {
                LogController.Error($"Error updating database: {ex.Message}");
                return;
            }
            
        }
    }



}
