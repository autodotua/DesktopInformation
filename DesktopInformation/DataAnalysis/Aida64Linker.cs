using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DesktopInformation.DataAnalysis
{
    public class Aida64Linker
    {
        RegistryKey rk;
        static int a = 0;
        public Aida64Linker()
        {
            StartLinkThread();
        }
        bool isLinking = false;
        public void StartLinkThread()
        {
            if (!isLinking)
            {
                Task.Run(async () =>
                {
                    isLinking = true;
                    bool aidaNotOpen = false;
                    while ((rk = Registry.CurrentUser.OpenSubKey(@"Software\FinalWire\AIDA64\SensorValues")) == null)
                    {
                        aidaNotOpen = true;
                        await Task.Delay(2000);
                    }
                    SupportValueName = rk.GetValueNames()
                 .Where(p => p.Contains("Value."))
                 .Select(p => p = p.Replace("Value.", "")).ToArray();
                    if (aidaNotOpen)
                    {
                        await App.Instance.Dispatcher.Invoke(App.Instance.Manager.RefreshWindows);
                    }
                    isLinking = false;
                });
            }
        }

        public string[] SupportValueName { get; private set; }

        public string GetValue(string key)
        {
            if (rk == null)
            {
                return "(AIDA64连接失败)";// null;
            }
      
            try
            {
                return rk.GetValue("Value." + key).ToString();
            }
            catch (Exception ex)
            {
                rk = null;
                StartLinkThread();
                return "(AIDA64连接失败)";// null;
            }
       
        }

        public static string[] GetSupportValueName()
        {
            return Registry.CurrentUser.OpenSubKey(@"Software\FinalWire\AIDA64\SensorValues") == null ? Array.Empty<string>() :
            Registry.CurrentUser.OpenSubKey(@"Software\FinalWire\AIDA64\SensorValues").GetValueNames()
                .Where(p => p.Contains("Value."))
                .Select(p => p = p.Replace("Value.", "")).ToArray();
        }
    }
}


/*

    External Applications 

The Hardware Monitoring feature of AIDA64 Extreme provides the following 3 methods to expose measured sensor values and other system values to external applications: 

Shared Memory 

Registry 

WMI 

A complete list of sensor value IDs and their meaning is below. 

System 

SDATE 
 Date 
 
STIME 
 Time 
 
STIMENS 
 Time (HH:MM) 
 
SUPTIME 
 UpTime 
 
SUPTIMENS 
 UpTime (HH:MM) 
 
SCPUCLK 
 CPU Clock 
 
SCC-1-01 
 CC-1-1 (CPU #1 / Core #1 Clock) 
 
SCC-1-02 
 CC-1-2 (CPU #1 / Core #2 Clock) 
 
....... 
 ............................... 
 
SCC-1-32 
 CC-1-32 (CPU #1 / Core #32 Clock) 
 
SCC-2-01 
 CC-2-1 (CPU #2 / Core #1 Clock) 
 
SCC-2-02 
 CC-2-2 (CPU #2 / Core #2 Clock) 
 
....... 
 ............................... 
 
SCC-2-32 
 CC-2-32 (CPU #2 / Core #32 Clock) 
 
SCPUMUL 
 CPU Multiplier 
 
SCPUFSB 
 CPU FSB 
 
SHTMUL 
 HyperTransport Multiplier 
 
SHTCLK 
 HyperTransport Clock 
 
SNBMUL 
 North Bridge Multiplier 
 
SNBCLK 
 North Bridge Clock 
 
SSAMUL 
 System Agent Multiplier 
 
SSACLK 
 System Agent Clock 
 
SMEMCLK 
 Memory Clock 
 
SMEMSPEED 
 Memory Speed 
 
SDRAMFSB 
 DRAM:FSB Ratio 
 
SMEMTIM 
 Memory Timings 
 
SBIOSVER 
 BIOS Version 
 
SCPUUTI 
 CPU Utilization 
 
SCPU1UTI 
 CPU1 Utilization 
 
SCPU2UTI 
 CPU2 Utilization 
 
......... 
 ................. 
 
SCPU80UTI 
 CPU80 Utilization 
 
SCPUTHR 
 CPU Throttling 
 
SMEMUTI 
 Memory Utilization 
 
SUSEDMEM 
 Used Memory 
 
SFREEMEM 
 Free Memory 
 
SPROCESSES 
 Processes 
 
SUSERS 
 Users 
 
SDRVAUTI 
 Drive A: Utilization 
 
SDRVAUSEDSPC 
 Drive A: Used Space 
 
SDRVAFREESPC 
 Drive A: Free Space 
 
SDRVBUTI 
 Drive B: Utilization 
 
SDRVBUSEDSPC 
 Drive B: Used Space 
 
SDRVBFREESPC 
 Drive B: Free Space 
 
............ 
 .................... 
 
SDRVZUTI 
 Drive Z: Utilization 
 
SDRVZUSEDSPC 
 Drive Z: Used Space 
 
SDRVZFREESPC 
 Drive Z: Free Space 
 
SSMASTA 
 SMART Status 
 
SDSK1ACT 
 Disk 1 Activity 
 
SDSK1READSPD 
 Disk 1 Read Speed 
 
SDSK1WRITESPD 
 Disk 1 Write Speed 
 
SDSK2ACT 
 Disk 2 Activity 
 
SDSK2READSPD 
 Disk 2 Read Speed 
 
SDSK2WRITESPD 
 Disk 2 Write Speed 
 
............. 
 .................... 
 
SDSK25ACT 
 Disk 25 Activity 
 
SDSK25READSPD 
 Disk 25 Read Speed 
 
SDSK25WRITESPD 
 Disk 25 Write Speed 
 
SGPU1CLK 
 GPU1 Clock 
 
SGPU1SHDCLK 
 GPU1 Shader Clock 
 
SGPU1MEMCLK 
 GPU1 Memory Clock 
 
SGPU1UTI 
 GPU1 Utilization 
 
SGPU1MCUTI 
 GPU1 MC Utilization 
 
SGPU1VEUTI 
 GPU1 VE Utilization 
 
SGPU1USEDDEMEM 
 GPU1 Used Dedicated Memory 
 
SGPU1USEDDYMEM 
 GPU1 Used Dynamic Memory 
 
SGPU1BUSTYP 
 GPU1 Bus Type 
 
SGPU1PWRCTRL 
 GPU1 PowerControl 
 
SGPU2CLK 
 GPU2 Clock 
 
SGPU2SHDCLK 
 GPU2 Shader Clock 
 
SGPU2MEMCLK 
 GPU2 Memory Clock 
 
SGPU2UTI 
 GPU2 Utilization 
 
SGPU2MCUTI 
 GPU2 MC Utilization 
 
SGPU2VEUTI 
 GPU2 VE Utilization 
 
SGPU2USEDDEMEM 
 GPU2 Used Dedicated Memory 
 
SGPU2USEDDYMEM 
 GPU2 Used Dynamic Memory 
 
SGPU2BUSTYP 
 GPU2 Bus Type 
 
SGPU2PWRCTRL 
 GPU2 PowerControl 
 
SGPU3CLK 
 GPU3 Clock 
 
SGPU3SHDCLK 
 GPU3 Shader Clock 
 
SGPU3MEMCLK 
 GPU3 Memory Clock 
 
SGPU3UTI 
 GPU3 Utilization 
 
SGPU3MCUTI 
 GPU3 MC Utilization 
 
SGPU3VEUTI 
 GPU3 VE Utilization 
 
SGPU3USEDDEMEM 
 GPU3 Used Dedicated Memory 
 
SGPU3USEDDYMEM 
 GPU3 Used Dynamic Memory 
 
SGPU3BUSTYP 
 GPU3 Bus Type 
 
SGPU3PWRCTRL 
 GPU3 PowerControl 
 
SGPU4CLK 
 GPU4 Clock 
 
SGPU4SHDCLK 
 GPU4 Shader Clock 
 
SGPU4MEMCLK 
 GPU4 Memory Clock 
 
SGPU4UTI 
 GPU4 Utilization 
 
SGPU4MCUTI 
 GPU4 MC Utilization 
 
SGPU4VEUTI 
 GPU4 VE Utilization 
 
SGPU4USEDDEMEM 
 GPU4 Used Dedicated Memory 
 
SGPU4USEDDYMEM 
 GPU4 Used Dynamic Memory 
 
SGPU4BUSTYP 
 GPU4 Bus Type 
 
SGPU4PWRCTRL 
 GPU4 PowerControl 
 
SGPU5CLK 
 GPU5 Clock 
 
SGPU5SHDCLK 
 GPU5 Shader Clock 
 
SGPU5MEMCLK 
 GPU5 Memory Clock 
 
SGPU5UTI 
 GPU5 Utilization 
 
SGPU5MCUTI 
 GPU5 MC Utilization 
 
SGPU5VEUTI 
 GPU5 VE Utilization 
 
SGPU5USEDDEMEM 
 GPU5 Used Dedicated Memory 
 
SGPU5USEDDYMEM 
 GPU5 Used Dynamic Memory 
 
SGPU5BUSTYP 
 GPU5 Bus Type 
 
SGPU5PWRCTRL 
 GPU5 PowerControl 
 
SGPU6CLK 
 GPU6 Clock 
 
SGPU6SHDCLK 
 GPU6 Shader Clock 
 
SGPU6MEMCLK 
 GPU6 Memory Clock 
 
SGPU6UTI 
 GPU6 Utilization 
 
SGPU6MCUTI 
 GPU6 MC Utilization 
 
SGPU6VEUTI 
 GPU6 VE Utilization 
 
SGPU6USEDDEMEM 
 GPU6 Used Dedicated Memory 
 
SGPU6USEDDYMEM 
 GPU6 Used Dynamic Memory 
 
SGPU6BUSTYP 
 GPU6 Bus Type 
 
SGPU6PWRCTRL 
 GPU6 PowerControl 
 
SGPU7CLK 
 GPU7 Clock 
 
SGPU7SHDCLK 
 GPU7 Shader Clock 
 
SGPU7MEMCLK 
 GPU7 Memory Clock 
 
SGPU7UTI 
 GPU7 Utilization 
 
SGPU7MCUTI 
 GPU7 MC Utilization 
 
SGPU7VEUTI 
 GPU7 VE Utilization 
 
SGPU7USEDDEMEM 
 GPU7 Used Dedicated Memory 
 
SGPU7USEDDYMEM 
 GPU7 Used Dynamic Memory 
 
SGPU7BUSTYP 
 GPU7 Bus Type 
 
SGPU7PWRCTRL 
 GPU7 PowerControl 
 
SGPU8CLK 
 GPU8 Clock 
 
SGPU8SHDCLK 
 GPU8 Shader Clock 
 
SGPU8MEMCLK 
 GPU8 Memory Clock 
 
SGPU8UTI 
 GPU8 Utilization 
 
SGPU8MCUTI 
 GPU8 MC Utilization 
 
SGPU8VEUTI 
 GPU8 VE Utilization 
 
SGPU8USEDDEMEM 
 GPU8 Used Dedicated Memory 
 
SGPU8USEDDYMEM 
 GPU8 Used Dynamic Memory 
 
SGPU8BUSTYP 
 GPU8 Bus Type 
 
SGPU8PWRCTRL 
 GPU8 PowerControl 
 
SVMEMUSAGE 
 Video Memory Utilization 
 
SUSEDVMEM 
 Used Video Memory 
 
SUSEDLVMEM 
 Used Local Video Memory 
 
SUSEDNLVMEM 
 Used Non-Local Video Memory 
 
SFREEVMEM 
 Free Video Memory 
 
SFREELVMEM 
 Free Local Video Memory 
 
SFREENLVMEM 
 Free Non-Local Video Memory 
 
SPRIIPADDR 
 Primary IP Address 
 
SEXTIPADDR 
 External IP Address 
 
SNIC1DLRATE 
 NIC1 Download Rate 
 
SNIC1ULRATE 
 NIC1 Upload Rate 
 
SNIC1TOTDL 
 NIC1 Total Download 
 
SNIC1TOTUL 
 NIC1 Total Upload 
 
SNIC1CONNSPD 
 NIC1 Connection Speed 
 
SNIC1WLANRSSI 
 NIC1 WLAN Signal Strength 
 
SNIC2DLRATE 
 NIC2 Download Rate 
 
SNIC2ULRATE 
 NIC2 Upload Rate 
 
SNIC2TOTDL 
 NIC2 Total Download 
 
SNIC2TOTUL 
 NIC2 Total Upload 
 
SNIC2CONNSPD 
 NIC2 Connection Speed 
 
SNIC2WLANRSSI 
 NIC2 WLAN Signal Strength 
 
SNIC3DLRATE 
 NIC3 Download Rate 
 
SNIC3ULRATE 
 NIC3 Upload Rate 
 
SNIC3TOTDL 
 NIC3 Total Download 
 
SNIC3TOTUL 
 NIC3 Total Upload 
 
SNIC3CONNSPD 
 NIC3 Connection Speed 
 
SNIC3WLANRSSI 
 NIC3 WLAN Signal Strength 
 
SNIC4DLRATE 
 NIC4 Download Rate 
 
SNIC4ULRATE 
 NIC4 Upload Rate 
 
SNIC4TOTDL 
 NIC4 Total Download 
 
SNIC4TOTUL 
 NIC4 Total Upload 
 
SNIC4CONNSPD 
 NIC4 Connection Speed 
 
SNIC4WLANRSSI 
 NIC4 WLAN Signal Strength 
 
SNIC5DLRATE 
 NIC5 Download Rate 
 
SNIC5ULRATE 
 NIC5 Upload Rate 
 
SNIC5TOTDL 
 NIC5 Total Download 
 
SNIC5TOTUL 
 NIC5 Total Upload 
 
SNIC5CONNSPD 
 NIC5 Connection Speed 
 
SNIC5WLANRSSI 
 NIC5 WLAN Signal Strength 
 
SNIC6DLRATE 
 NIC6 Download Rate 
 
SNIC6ULRATE 
 NIC6 Upload Rate 
 
SNIC6TOTDL 
 NIC6 Total Download 
 
SNIC6TOTUL 
 NIC6 Total Upload 
 
SNIC6CONNSPD 
 NIC6 Connection Speed 
 
SNIC6WLANRSSI 
 NIC6 WLAN Signal Strength 
 
SNIC7DLRATE 
 NIC7 Download Rate 
 
SNIC7ULRATE 
 NIC7 Upload Rate 
 
SNIC7TOTDL 
 NIC7 Total Download 
 
SNIC7TOTUL 
 NIC7 Total Upload 
 
SNIC7CONNSPD 
 NIC7 Connection Speed 
 
SNIC7WLANRSSI 
 NIC7 WLAN Signal Strength 
 
SNIC8DLRATE 
 NIC8 Download Rate 
 
SNIC8ULRATE 
 NIC8 Upload Rate 
 
SNIC8TOTDL 
 NIC8 Total Download 
 
SNIC8TOTUL 
 NIC8 Total Upload 
 
SNIC8CONNSPD 
 NIC8 Connection Speed 
 
SNIC8WLANRSSI 
 NIC8 WLAN Signal Strength 
 
SDESKRES 
 Desktop Resolution 
 
SVREFRATE 
 Vertical Refresh Rate 
 
SDISPBRILVL 
 Display Brightness Level 
 
SMASTVOL 
 Master Volume 
 
SMEDTIT 
 Media Title 
 
SMEDSTA 
 Media Status 
 
SMEDPOS 
 Media Position 
 
SBATTLVL 
 Battery Level 
 
SBATT 
 Battery 
 
SESTBATTTIME 
 Estimated Battery Time 
 
SPWRSTATE 
 Power State 
 
SBATTPWRLOADPERC 
 Battery Power Load 
 
SFRAPS 
 Fraps 
 
SRTSSFPS 
 RTSS FPS 
 
SJDDLRATE 
 JD Download Rate 
 
SJDTOTDL 
 JD Total Download 
 
SJDREMDL 
 JD Remaining Download 
 
SJDETA 
 JD ETA 
 
SREGVALS1 
 Registry Value Str1 
 
SREGVALS2 
 Registry Value Str2 
 
SREGVALS3 
 Registry Value Str3 
 
SREGVALS4 
 Registry Value Str4 
 
SREGVALS5 
 Registry Value Str5 
 
SREGVALS6 
 Registry Value Str6 
 
SREGVALS7 
 Registry Value Str7 
 
SREGVALS8 
 Registry Value Str8 
 
SREGVALS9 
 Registry Value Str9 
 
SREGVALS10 
 Registry Value Str10 
 
SREGVALD1 
 Registry Value DW1 
 
SREGVALD2 
 Registry Value DW2 
 
SREGVALD3 
 Registry Value DW3 
 
SREGVALD4 
 Registry Value DW4 
 
SREGVALD5 
 Registry Value DW5 
 
SREGVALD6 
 Registry Value DW6 
 
SREGVALD7 
 Registry Value DW7 
 
SREGVALD8 
 Registry Value DW8 
 
SREGVALD9 
 Registry Value DW9 
 
SREGVALD10 
 Registry Value DW10 
 


Temperatures 

TMOBO 
 Motherboard 
 
TCPU 
 CPU 
 
TCPU1 
 CPU1 
 
TCPU2 
 CPU2 
 
TCPU3 
 CPU3 
 
TCPU4 
 CPU4 
 
TCPUDIO 
 CPU Diode 
 
TCPUPKG 
 CPU Package 
 
TCPUIAC 
 CPU IA Cores 
 
TCPUGTC 
 CPU GT Cores 
 
TCC-1-1 
 CC-1-1 (CPU #1 / Core #1) 
 
TCC-1-2 
 CC-1-2 (CPU #1 / Core #2) 
 
........ 
 ......................... 
 
TCC-1-32 
 CC-1-32 (CPU #1 / Core #32) 
 
TCC-2-1 
 CC-2-1 (CPU #2 / Core #1) 
 
TCC-2-2 
 CC-2-2 (CPU #2 / Core #2) 
 
........ 
 ......................... 
 
TCC-2-32 
 CC-2-32 (CPU #2 / Core #32) 
 
TPPGACPU 
 PPGA CPU 
 
TS1CPU 
 Slot1 CPU 
 
TDIMM 
 DIMM 
 
TDIMM1 
 DIMM1 
 
TDIMM2 
 DIMM2 
 
TAGP 
 AGP 
 
TMPCI 
 MiniPCI 
 
TPCMCIA 
 PCMCIA 
 
TPCIE 
 PCI-E 
 
TPCIE1 
 PCI-E #1 
 
TPCIE2 
 PCI-E #2 
 
TPCIE3 
 PCI-E #3 
 
TUSB30 
 USB 3.0 
 
TUSB301 
 USB 3.0 #1 
 
TUSB302 
 USB 3.0 #2 
 
TUSB31 
 USB 3.1 
 
TSATA6G 
 SATA 6G 
 
TMXM 
 MXM 
 
TSOC 
 SoC 
 
TCHIP  
 Chipset 
 
TNB 
 North Bridge 
 
TSB 
 South Bridge 
 
TPCH 
 PCH 
 
TPCHCORE 
 PCH Core 
 
TPCHDIO 
 PCH Diode 
 
TIMC 
 IMC 
 
TMCP 
 MCP 
 
TGMCH 
 GMCH 
 
TGMCH1 
 GMCH1 
 
TGMCH2 
 GMCH2 
 
TPXH 
 PXH 
 
TPLX 
 PLX 
 
TPSU 
 Power Supply 
 
TPSU1 
 Power Supply #1 
 
TPSU2 
 Power Supply #2 
 
TPSU3 
 Power Supply #3 
 
TAPS 
 APS 
 
TODD 
 Optical Drive 
 
TWLAN 
 WLAN 
 
TLCD 
 LCD 
 
TIGPU 
 iGPU 
 
TRAIDCTR 
 RAID Controller 
 
TRAIDCTR1 
 RAID Controller #1 
 
TRAIDCTR2 
 RAID Controller #2 
 
TRAIDCTR3 
 RAID Controller #3 
 
TRAIDCTR4 
 RAID Controller #4 
 
TWATER 
 Water 
 
TBATT 
 Battery 
 
TBATT2 
 Battery #2 
 
TPWM 
 PWM 
 
TPWM1 
 PWM1 
 
TPWM2 
 PWM2 
 
TPWM3 
 PWM3 
 
TPWM4 
 PWM4 
 
TPWM5 
 PWM5 
 
TVRM 
 VRM 
 
TVRM1 
 VRM1 
 
TVRM2 
 VRM2 
 
TVRM3 
 VRM3 
 
TAUX 
 Aux 
 
TFRONT 
 Front 
 
TREAR 
 Rear 
 
TVCCIO 
 VCCIO 
 
TVCCSA 
 VCCSA 
 
TOPT1 
 OPT1 
 
TOPT2 
 OPT2 
 
TOPT3 
 OPT3 
 
TSZS1 
 Subzero Sense #1 
 
TSZS2 
 Subzero Sense #2 
 
TFAN1VRM 
 Fan #1 VRM 
 
TFAN2VRM 
 Fan #2 VRM 
 
TFAN3VRM 
 Fan #3 VRM 
 
TFAN4VRM 
 Fan #4 VRM 
 
TTEMP1 
 Temperature #1 
 
TTEMP2 
 Temperature #2 
 
....... 
 ............... 
 
TTEMP99 
 Temperature #99 
 
TGPU1 
 GPU1 
 
TGPU1DIO 
 GPU1 Diode 
 
TGPU1DIOD 
 GPU1 Diode (DispIO) 
 
TGPU1DIOM 
 GPU1 Diode (MemIO) 
 
TGPU1DIOS 
 GPU1 Diode (Shader) 
 
TGPU1AMB 
 GPU1 Ambient 
 
TGPU1MEM 
 GPU1 Memory 
 
TGPU1VRM 
 GPU1 VRM 
 
TGPU1VRM1 
 GPU1 VRM1 
 
TGPU1VRM2 
 GPU1 VRM2 
 
TGPU2 
 GPU2 
 
TGPU2DIO 
 GPU2 Diode 
 
TGPU2DIOD 
 GPU2 Diode (DispIO) 
 
TGPU2DIOM 
 GPU2 Diode (MemIO) 
 
TGPU2DIOS 
 GPU2 Diode (Shader) 
 
TGPU2AMB 
 GPU2 Ambient 
 
TGPU2MEM 
 GPU2 Memory 
 
TGPU2VRM 
 GPU2 VRM 
 
TGPU2VRM1 
 GPU2 VRM1 
 
TGPU2VRM2 
 GPU2 VRM2 
 
TGPU3 
 GPU3 
 
TGPU3DIO 
 GPU3 Diode 
 
TGPU3DIOD 
 GPU3 Diode (DispIO) 
 
TGPU3DIOM 
 GPU3 Diode (MemIO) 
 
TGPU3DIOS 
 GPU3 Diode (Shader) 
 
TGPU3AMB 
 GPU3 Ambient 
 
TGPU3MEM 
 GPU3 Memory 
 
TGPU3VRM 
 GPU3 VRM 
 
TGPU3VRM1 
 GPU3 VRM1 
 
TGPU3VRM2 
 GPU3 VRM2 
 
TGPU4 
 GPU4 
 
TGPU4DIO 
 GPU4 Diode 
 
TGPU4DIOD 
 GPU4 Diode (DispIO) 
 
TGPU4DIOM 
 GPU4 Diode (MemIO) 
 
TGPU4DIOS 
 GPU4 Diode (Shader) 
 
TGPU4AMB 
 GPU4 Ambient 
 
TGPU4MEM 
 GPU4 Memory 
 
TGPU4VRM 
 GPU4 VRM 
 
TGPU4VRM1 
 GPU4 VRM1 
 
TGPU4VRM2 
 GPU4 VRM2 
 
TGPU5 
 GPU5 
 
TGPU5DIO 
 GPU5 Diode 
 
TGPU5DIOD 
 GPU5 Diode (DispIO) 
 
TGPU5DIOM 
 GPU5 Diode (MemIO) 
 
TGPU5DIOS 
 GPU5 Diode (Shader) 
 
TGPU5AMB 
 GPU5 Ambient 
 
TGPU5MEM 
 GPU5 Memory 
 
TGPU5VRM 
 GPU5 VRM 
 
TGPU5VRM1 
 GPU5 VRM1 
 
TGPU5VRM2 
 GPU5 VRM2 
 
TGPU6 
 GPU6 
 
TGPU6DIO 
 GPU6 Diode 
 
TGPU6DIOD 
 GPU6 Diode (DispIO) 
 
TGPU6DIOM 
 GPU6 Diode (MemIO) 
 
TGPU6DIOS 
 GPU6 Diode (Shader) 
 
TGPU6AMB 
 GPU6 Ambient 
 
TGPU6MEM 
 GPU6 Memory 
 
TGPU6VRM 
 GPU6 VRM 
 
TGPU6VRM1 
 GPU6 VRM1 
 
TGPU6VRM2 
 GPU6 VRM2 
 
TGPU7 
 GPU7 
 
TGPU7DIO 
 GPU7 Diode 
 
TGPU7DIOD 
 GPU7 Diode (DispIO) 
 
TGPU7DIOM 
 GPU7 Diode (MemIO) 
 
TGPU7DIOS 
 GPU7 Diode (Shader) 
 
TGPU7AMB 
 GPU7 Ambient 
 
TGPU7MEM 
 GPU7 Memory 
 
TGPU7VRM 
 GPU7 VRM 
 
TGPU7VRM1 
 GPU7 VRM1 
 
TGPU7VRM2 
 GPU7 VRM2 
 
TGPU8 
 GPU8 
 
TGPU8DIO 
 GPU8 Diode 
 
TGPU8DIOD 
 GPU8 Diode (DispIO) 
 
TGPU8DIOM 
 GPU8 Diode (MemIO) 
 
TGPU8DIOS 
 GPU8 Diode (Shader) 
 
TGPU8AMB 
 GPU8 Ambient 
 
TGPU8MEM 
 GPU8 Memory 
 
TGPU8VRM 
 GPU8 VRM 
 
TGPU8VRM1 
 GPU8 VRM1 
 
TGPU8VRM2 
 GPU8 VRM2 
 
TAMB1 
 1st FB-DIMM 
 
TAMB2 
 2nd FB-DIMM 
 
...... 
 ............ 
 
TAMB32 
 32nd FB-DIMM 
 
TDIMMTS1 
 1st DIMM 
 
TDIMMTS2 
 2nd DIMM 
 
......... 
 ......... 
 
TDIMMTS64 
 64th DIMM 
 
THDD1 
 1st HDD 
 
THDD2 
 2nd HDD 
 
...... 
 ........ 
 
THDD50 
 50th HDD 
 


Cooling Fans 

FCPU 
 CPU 
 
FCPU1 
 CPU1 
 
FCPU2 
 CPU2 
 
FCPU3 
 CPU3 
 
FCPU4 
 CPU4 
 
FCPUOPT 
 CPU OPT 
 
FSYS 
 System 
 
FCHIP 
 Chipset 
 
FNB 
 North Bridge 
 
FSB 
 South Bridge 
 
FPCH 
 PCH 
 
FNFORCE 
 nForce 
 
FCHA 
 Chassis 
 
FCHA1 
 Chassis #1 
 
FCHA2 
 Chassis #2 
 
..... 
 .......... 
 
FCHA9 
 Chassis #9 
 
FPSU 
 Power Supply 
 
FFRONT 
 Front 
 
FFRONT1 
 Front #1 
 
FFRONT2 
 Front #2 
 
FFRONT3 
 Front #3 
 
FFRONT4 
 Front #4 
 
FREAR 
 Rear 
 
FREAR1 
 Rear #1 
 
FREAR2 
 Rear #2 
 
FOTES 
 OTES 
 
FOTES1 
 OTES1 
 
FOTES2 
 OTES2 
 
FDIMM 
 DIMM 
 
FFBD 
 FBD 
 
FFBD1 
 FBD1 
 
FFBD2 
 FBD2 
 
FHDD 
 HDD 
 
FODD 
 ODD 
 
FMXM 
 MXM 
 
FPWM 
 PWM 
 
FHAMP 
 HAMP 
 
FWPUMP 
 Water Pump 
 
FPUMP1 
 Pump #1 
 
FPUMP2 
 Pump #2 
 
...... 
 ....... 
 
FPUMP8 
 Pump #8 
 
FASSIST 
 Assistant 
 
FASSIST1 
 Assistant #1 
 
FASSIST2 
 Assistant #2 
 
FASSIST3 
 Assistant #3 
 
FAUX 
 Aux 
 
FAUX1 
 Aux1 
 
FAUX2 
 Aux2 
 
FAUX3 
 Aux3 
 
FAUX4 
 Aux4 
 
FAUX5 
 Aux5 
 
FOPT1 
 OPT1 
 
FOPT2 
 OPT2 
 
FOPT3 
 OPT3 
 
FOPT4 
 OPT4 
 
FOPT5 
 OPT5 
 
FFAN1 
 Fan #1 
 
FFAN2 
 Fan #2 
 
...... 
 ....... 
 
FFAN40 
 Fan #40 
 
FGPU1 
 GPU1 
 
FGPU2 
 GPU2 
 
FGPU3 
 GPU3 
 
FGPU4 
 GPU4 
 
FGPU5 
 GPU5 
 
FGPU6 
 GPU6 
 
FGPU7 
 GPU7 
 
FGPU8 
 GPU8 
 


Fan Duty Cycles 

DCPU 
 CPU 
 
DSYS 
 System 
 
DTBAL1 
 T-Balancer #1 
 
DTBAL2 
 T-Balancer #2 
 
DTBAL3 
 T-Balancer #3 
 
DTBAL4 
 T-Balancer #4 
 
DGPU1 
 GPU1 
 
DGPU2 
 GPU2 
 
DGPU3 
 GPU3 
 
DGPU4 
 GPU4 
 
DGPU5 
 GPU5 
 
DGPU6 
 GPU6 
 
DGPU7 
 GPU7 
 
DGPU8 
 GPU8 
 


Voltage Values 

VCPU 
 CPU Core 
 
VCPU1 
 CPU1 Core 
 
VCPU2 
 CPU2 Core 
 
VCPU3 
 CPU3 Core 
 
VCPU4 
 CPU4 Core 
 
VCPUVRM 
 CPU VRM 
 
VCPU1VRM 
 CPU1 VRM 
 
VCPU2VRM 
 CPU2 VRM 
 
VCPU3VRM 
 CPU3 VRM 
 
VCPU4VRM 
 CPU4 VRM 
 
VCPUVID 
 CPU VID 
 
V09V 
 +0.9 V 
 
V105V 
 +1.05 V 
 
V11V 
 +1.1 V 
 
V12V 
 +1.2 V 
 
V125V 
 +1.25 V 
 
V13V 
 +1.3 V 
 
V15V 
 +1.5 V 
 
V18V 
 +1.8 V 
 
V25V 
 +2.5 V 
 
V26V 
 +2.6 V 
 
V33V 
 +3.3 V 
 
VP5V 
 +5 V 
 
VM5V 
 -5 V 
 
VP12V 
 +12 V 
 
VP12V1 
 +12 V #1 
 
VP12V2 
 +12 V #2 
 
VP12V3 
 +12 V #3 
 
VP12V4 
 +12 V #4 
 
VP12VCPU1 
 +12 V CPU1 
 
VP12VCPU2 
 +12 V CPU2 
 
VP12V4P 
 +12 V 4-pin 
 
VP12V8P 
 +12 V 8-pin 
 
VP12VVRM 
 +12 V VRM 
 
VP12VVRM1 
 +12 V VRM1 
 
VP12VVRM2 
 +12 V VRM2 
 
VM12V 
 -12 V 
 
VBAT 
 VBAT Battery 
 
V3VSB 
 +3.3 V Standby 
 
V5VSB 
 +5 V Standby 
 
VDIMM 
 DIMM 
 
VDIMMAB 
 DIMM AB 
 
VDIMMCD 
 DIMM CD 
 
VDIMMEF 
 DIMM EF 
 
VDIMMGH 
 DIMM GH 
 
VCPU1DIMM 
 CPU1 DIMM 
 
VCPU2DIMM 
 CPU2 DIMM 
 
VRIMM 
 RIMM 
 
VDIMMVTT 
 DIMM VTT 
 
VCPU1DVTT 
 CPU1 DIMM VTT 
 
VCPU2DVTT 
 CPU2 DIMM VTT 
 
VAGP 
 AGP 
 
VAGPVDDQ 
 AGP VDDQ 
 
VCHIP 
 Chipset 
 
VCPUNB 
 CPU/NB 
 
VNBCORE 
 North Bridge Core 
 
VNBVID 
 North Bridge VID 
 
VNBPLL 
 North Bridge PLL 
 
VNB11V 
 North Bridge +1.1 V 
 
VNB12V 
 North Bridge +1.2 V 
 
VNB15V 
 North Bridge +1.5 V 
 
VNB18V 
 North Bridge +1.8 V 
 
VNB20V 
 North Bridge +2.0 V 
 
VNB25V 
 North Bridge +2.5 V 
 
VSBCORE 
 South Bridge Core 
 
VSBPLL 
 South Bridge PLL 
 
VSB11V 
 South Bridge +1.1 V 
 
VSB12V 
 South Bridge +1.2 V 
 
VSB15V 
 South Bridge +1.5 V 
 
VPCHCORE 
 PCH Core 
 
VPCHIO 
 PCH I/O 
 
VPCHPLL 
 PCH PLL 
 
VPCH10V 
 PCH +1.0 V 
 
VPCH11V 
 PCH +1.1 V 
 
VPCH15V 
 PCH +1.5 V 
 
VPCH18V 
 PCH +1.8 V 
 
VPCIE 
 PCI Express 
 
VHT 
 HyperTransport 
 
VQPI 
 QPI 
 
VIMC 
 IMC 
 
VSOC 
 SoC 
 
VVDD 
 VDD 
 
VVDDA 
 VDDA 
 
VVDDP 
 VDDP 
 
VVPPM 
 VPPM 
 
VCPUVDD 
 CPU VDD 
 
VCPUVDDNB 
 CPU VDDNB 
 
VCPUCAC 
 CPU Cache 
 
VVGTL 
 VGTL 
 
VVBT 
 VBT 
 
VVTT 
 VTT 
 
VCPUVTT 
 CPU VTT 
 
VCPUVTT2 
 CPU VTT #2 
 
VCPU1VTT 
 CPU1 VTT 
 
VCPU2VTT 
 CPU2 VTT 
 
VCPUPLL 
 CPU PLL 
 
VFSBVTT 
 FSB VTT 
 
VGMCHVTT 
 GMCH VTT 
 
VVCCIO 
 VCCIO 
 
VVCCSA 
 VCCSA 
 
VCPU1VCCSA 
 CPU1 VCCSA 
 
VCPU2VCCSA 
 CPU2 VCCSA 
 
VVCCST 
 VCC Sustain 
 
VVTR 
 VTR 
 
V5VTR 
 5VTR 
 
VNIC 
 Network Adapter 
 
VESATA 
 eSATA 
 
VPSU 
 Power Supply 
 
VIGPU 
 iGPU 
 
VFAN21 
 Fan #21 
 
VFAN22 
 Fan #22 
 
...... 
 ....... 
 
VFAN32 
 Fan #32 
 
VPUMP1 
 Pump #1 
 
VPUMP2 
 Pump #2 
 
VBATT 
 Battery 
 
VBATTINP 
 Battery Input 
 
VBATTOUTP 
 Battery Output 
 
VBATT2 
 Battery #2 
 
VBATT2INP 
 Battery #2 Input 
 
VBATT2OUTP 
 Battery #2 Output 
 
VGPU1 
 GPU1 Core 
 
VGPU1VCC 
 GPU1 Vcc 
 
VGPU1MEM 
 GPU1 Memory 
 
VGPU1VRM 
 GPU1 VRM 
 
VGPU1P12V 
 GPU1 +12V 
 
VGPU2 
 GPU2 Core 
 
VGPU2VCC 
 GPU2 Vcc 
 
VGPU2MEM 
 GPU2 Memory 
 
VGPU2VRM 
 GPU2 VRM 
 
VGPU2P12V 
 GPU2 +12V 
 
VGPU3 
 GPU3 Core 
 
VGPU3VCC 
 GPU3 Vcc 
 
VGPU3MEM 
 GPU3 Memory 
 
VGPU3VRM 
 GPU3 VRM 
 
VGPU3P12V 
 GPU3 +12V 
 
VGPU4 
 GPU4 Core 
 
VGPU4VCC 
 GPU4 Vcc 
 
VGPU4MEM 
 GPU4 Memory 
 
VGPU4VRM 
 GPU4 VRM 
 
VGPU4P12V 
 GPU4 +12V 
 
VGPU5 
 GPU5 Core 
 
VGPU5VCC 
 GPU5 Vcc 
 
VGPU5MEM 
 GPU5 Memory 
 
VGPU5VRM 
 GPU5 VRM 
 
VGPU5P12V 
 GPU5 +12V 
 
VGPU6 
 GPU6 Core 
 
VGPU6VCC 
 GPU6 Vcc 
 
VGPU6MEM 
 GPU6 Memory 
 
VGPU6VRM 
 GPU6 VRM 
 
VGPU6P12V 
 GPU6 +12V 
 
VGPU7 
 GPU7 Core 
 
VGPU7VCC 
 GPU7 Vcc 
 
VGPU7MEM 
 GPU7 Memory 
 
VGPU7VRM 
 GPU7 VRM 
 
VGPU7P12V 
 GPU7 +12V 
 
VGPU8 
 GPU8 Core 
 
VGPU8VCC 
 GPU8 Vcc 
 
VGPU8MEM 
 GPU8 Memory 
 
VGPU8VRM 
 GPU8 VRM 
 
VGPU8P12V 
 GPU8 +12V 
 


Current Values 

CCPU 
 CPU 
 
CCPU1 
 CPU1 
 
CCPU2 
 CPU2 
 
CCPU3 
 CPU3 
 
CCPU4 
 CPU4 
 
CCPUVDD 
 CPU VDD 
 
CCPUVDDNB 
 CPU VDDNB 
 
C15V 
 +1.5 V 
 
C18V 
 +1.8 V 
 
C25V 
 +2.5 V 
 
C33V 
 +3.3 V 
 
CP5V 
 +5 V 
 
CM5V 
 -5 V 
 
CP12V 
 +12 V 
 
CP12V1 
 +12 V #1 
 
CP12V2 
 +12 V #2 
 
CP12V3 
 +12 V #3 
 
CP12V4 
 +12 V #4 
 
CM12V 
 -12 V 
 
CNB 
 North Bridge 
 
CPCH 
 PCH 
 
CPWM 
 PWM 
 
CVRM 
 VRM 
 
CFAN21 
 Fan #21 
 
CFAN22 
 Fan #22 
 
...... 
 ....... 
 
CFAN32 
 Fan #32 
 
CPUMP1 
 Pump #1 
 
CPUMP2 
 Pump #2 
 
CPSU 
 Power Supply 
 
CBATTOUTP 
 Battery Output 
 
CBATT2OUTP 
 Battery #2 Output 
 
CGPU1MEM 
 GPU1 Memory 
 
CGPU1VRM 
 GPU1 VRM 
 
CGPU2MEM 
 GPU2 Memory 
 
CGPU2VRM 
 GPU2 VRM 
 
CGPU3MEM 
 GPU3 Memory 
 
CGPU3VRM 
 GPU3 VRM 
 
CGPU4MEM 
 GPU4 Memory 
 
CGPU4VRM 
 GPU4 VRM 
 
CGPU5MEM 
 GPU5 Memory 
 
CGPU5VRM 
 GPU5 VRM 
 
CGPU6MEM 
 GPU6 Memory 
 
CGPU6VRM 
 GPU6 VRM 
 
CGPU7MEM 
 GPU7 Memory 
 
CGPU7VRM 
 GPU7 VRM 
 
CGPU8MEM 
 GPU8 Memory 
 
CGPU8VRM 
 GPU8 VRM 
 


Power Values 

PCPU 
 CPU 
 
PCPU1 
 CPU1 
 
PCPU2 
 CPU2 
 
PCPUPKG 
 CPU Package 
 
PCPUIAC 
 CPU IA Cores 
 
PCPUGTC 
 CPU GT Cores 
 
PCPUCU0 
 CPU CU0 
 
PCPUCU1 
 CPU CU1 
 
PCPUUNC 
 CPU Uncore 
 
PCPUVDD 
 CPU VDD 
 
PCPUVDDNB 
 CPU VDDNB 
 
P15V 
 +1.5 V 
 
P33V 
 +3.3 V 
 
PP5V 
 +5 V 
 
PP12V 
 +12 V 
 
PP12V1 
 +12 V #1 
 
PP12V2 
 +12 V #2 
 
PP12V3 
 +12 V #3 
 
PP12V4 
 +12 V #4 
 
PDIMM 
 DIMM 
 
PNB 
 North Bridge 
 
PVRM 
 VRM 
 
PIGPU 
 iGPU 
 
PFAN21 
 Fan #21 
 
PFAN22 
 Fan #22 
 
...... 
 ....... 
 
PFAN32 
 Fan #32 
 
PPUMP1 
 Pump #1 
 
PPUMP2 
 Pump #2 
 
PPWR1 
 Power #1 
 
PPWR2 
 Power #2 
 
PPWR3 
 Power #3 
 
PPWR4 
 Power #4 
 
PPSU 
 Power Supply 
 
PBATT 
 Battery 
 
PBATTOUTP 
 Battery Output 
 
PBATTCHR 
 Battery Charge Rate 
 
PBATT2 
 Battery #2 
 
PBATT2OUTP 
 Battery #2 Output 
 
PBATT2CHR 
 Battery #2 Charge Rate 
 
PGPU1 
 GPU1 
 
PGPU1TDPP 
 GPU1 TDP% 
 
PGPU1MEM 
 GPU1 Memory 
 
PGPU1VRM 
 GPU1 VRM 
 
PGPU2 
 GPU2 
 
PGPU2TDPP 
 GPU2 TDP% 
 
PGPU2MEM 
 GPU2 Memory 
 
PGPU2VRM 
 GPU2 VRM 
 
PGPU3 
 GPU3 
 
PGPU3TDPP 
 GPU3 TDP% 
 
PGPU3MEM 
 GPU3 Memory 
 
PGPU3VRM 
 GPU3 VRM 
 
PGPU4 
 GPU4 
 
PGPU4TDPP 
 GPU4 TDP% 
 
PGPU4MEM 
 GPU4 Memory 
 
PGPU4VRM 
 GPU4 VRM 
 
PGPU5 
 GPU5 
 
PGPU5TDPP 
 GPU5 TDP% 
 
PGPU5MEM 
 GPU5 Memory 
 
PGPU5VRM 
 GPU5 VRM 
 
PGPU6 
 GPU6 
 
PGPU6TDPP 
 GPU6 TDP% 
 
PGPU6MEM 
 GPU6 Memory 
 
PGPU6VRM 
 GPU6 VRM 
 
PGPU7 
 GPU7 
 
PGPU7TDPP 
 GPU7 TDP% 
 
PGPU7MEM 
 GPU7 Memory 
 
PGPU7VRM 
 GPU7 VRM 
 
PGPU8 
 GPU8 
 
PGPU8TDPP 
 GPU8 TDP% 
 
PGPU8MEM 
 GPU8 Memory 
 
PGPU8VRM 
 GPU8 VRM 
 


Flow Sensors 

WFLOW1 
 Flow #1 
 
WFLOW2 
 Flow #2 
 
....... 
 ........ 
 
WFLOW20 
 Flow #20 
 


Liquid Levels 

LLIQ1 
 Liquid #1 
 
LLIQ2 
 Liquid #2 
 
LLIQ3 
 Liquid #3 
 
LLIQ4 
 Liquid #4 
 







*/
