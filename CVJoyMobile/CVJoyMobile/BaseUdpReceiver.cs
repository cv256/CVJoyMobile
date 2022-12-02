using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.ComponentModel;

public class BaseUdpReceiver
{

    public IPAddress ip = IPAddress.Any;
    public int port = 45000;

    public event EventHandler Updated;
    public event EventHandler UpdatedExtra;

    private UdpClient udpClient;

    public struct structInfo
    {
        public UInt16 speed;
        public String gear;
        public Boolean gearAuto;
        public UInt16 rpm;
        public Color slipFL;
        public Color slipFR;
        public Color slipRL;
        public Color slipRR;
        public Color dirtFL;
        public Color dirtFR;
        public Color dirtRL;
        public Color dirtRR;
        public Single accel;
        public Single brake;
        public Single clutch;
    }
    public structInfo Info;

    public struct structInfoExtra
    {
        public UInt16 rpmMax;
        public Color wearFL;
        public Color wearFR;
        public Color wearRL;
        public Color wearRR;
        public byte MaxFuel;
        public byte Fuel; // l
        public Single FuelAvg; // l/100KM
        public byte NumCars;
        public byte Position;
        public byte NumberOfLaps;
        public byte CompletedLaps;
        public Single DistanceTraveled; // KMs
    }
    public structInfoExtra InfoExtra;

    public BaseUdpReceiver()
    {
    }


    public void Start()
    {
        if (udpClient != null) { return; }

        udpClient = new UdpClient()
        {
            ExclusiveAddressUse = false,
            EnableBroadcast = true
        };
        udpClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        udpClient.Client.Bind(new IPEndPoint(ip, port));

        udpClient.BeginReceive(new AsyncCallback(OnUdpDataReceived), this);
    }


    public void End()
    {
        if (udpClient == null) { return; }
        udpClient.Close();
        udpClient.Dispose();
        udpClient = null;
    }


    public void StartDebug()
    {
        Xamarin.Forms.Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
        {
            byte[] recvBuffer = { 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (Int16 x = 1; x <= 23; x++)
            {
                recvBuffer[x] = (Byte)new Random().Next(255);
            }
            setData(recvBuffer);
            return true;
        });
    }


    private static void OnUdpDataReceived(IAsyncResult result)
    {
        BaseUdpReceiver thi = (BaseUdpReceiver)result.AsyncState;
        IPEndPoint remoteAddr = null;
        byte[] recvBuffer = null;
        try
        {
            recvBuffer = thi.udpClient.EndReceive(result, ref remoteAddr);
        }
        catch (Exception ex)
        {
            Debug.Print(ex.Message);
            return;
        }

        if (recvBuffer != null) { thi.setData(recvBuffer); }

        if (thi.udpClient != null) { thi.udpClient.BeginReceive(OnUdpDataReceived, thi); }
    }


    public void setData(byte[] recvBuffer)
    {
        if (recvBuffer == null) { return; }

        if (recvBuffer[0] == 255 && recvBuffer.Length >= 18)
        {
            Info.speed = Convert.ToUInt16(recvBuffer[1] + recvBuffer[2] * 256);
            Info.rpm = Convert.ToUInt16(recvBuffer[3] + recvBuffer[4] * 256);
            switch (recvBuffer[5])
            {
                case 0:
                    Info.gear = " ";
                    break;
                case 1:
                    Info.gear = "R";
                    break;
                case 2:
                    Info.gear = "N";
                    break;
                default:
                    Info.gear = (Convert.ToUInt16(recvBuffer[5]) - 2).ToString();
                    break;
            }
            Info.slipFL = Color.FromArgb(recvBuffer[6], 0, 0);
            Info.slipFR = Color.FromArgb(recvBuffer[7], 0, 0);
            Info.slipRL = Color.FromArgb(recvBuffer[8], 0, 0);
            Info.slipRR = Color.FromArgb(recvBuffer[9], 0, 0);
            Info.gearAuto = (recvBuffer[10] & 1) > 0;
            Info.dirtFL = Color.FromArgb(recvBuffer[11], 128, 128, 128);
            Info.dirtFR = Color.FromArgb(recvBuffer[12], 128, 128, 128);
            Info.dirtRL = Color.FromArgb(recvBuffer[13], 128, 128, 128);
            Info.dirtRR = Color.FromArgb(recvBuffer[14], 128, 128, 128);
            Info.accel = (Single)recvBuffer[15]/255;
            Info.brake = (Single)recvBuffer[16]/255;
            Info.clutch = (Single)recvBuffer[17]/255;
            if (recvBuffer.Length == 34)
            {
                InfoExtra.wearFL = Color.FromArgb(Math.Min(recvBuffer[18] * 4, 255), 255, 0, 0);
                InfoExtra.wearFR = Color.FromArgb(Math.Min(recvBuffer[19] * 4, 255), 255, 0, 0);
                InfoExtra.wearRL = Color.FromArgb(Math.Min(recvBuffer[20] * 4, 255), 255, 0, 0);
                InfoExtra.wearRR = Color.FromArgb(Math.Min(recvBuffer[21] * 1, 255), 255, 0, 0);
                InfoExtra.rpmMax = Convert.ToUInt16(recvBuffer[22] + recvBuffer[23] * 256);
                InfoExtra.MaxFuel = recvBuffer[24];
                InfoExtra.Fuel = recvBuffer[25];
                InfoExtra.NumCars = recvBuffer[26];
                InfoExtra.Position = recvBuffer[27];
                InfoExtra.NumberOfLaps = recvBuffer[28];
                InfoExtra.CompletedLaps = recvBuffer[29];
                InfoExtra.DistanceTraveled = (Single)(recvBuffer[30] + recvBuffer[31] * 256)/1000;
                InfoExtra.FuelAvg= (Single)(recvBuffer[32] + recvBuffer[33] * 256)/100;
                UpdatedExtra?.Invoke(this, new EventArgs());
            }
            Updated?.Invoke(this, new EventArgs());
        }
    }


    public double RpmPercent()
    {
        if (InfoExtra.rpmMax < Info.rpm)
        {
            InfoExtra.rpmMax = Info.rpm;
        }
        if (InfoExtra.rpmMax == 0)
        {
            return 0;
        }
        return (double)Info.rpm / (double)InfoExtra.rpmMax;
    }


    public Color RpmColor()
    {
        int r = (int)(RpmPercent() * 255);
        return Color.FromArgb(127 + Convert.ToInt16(r / 2), r , 255 - r , 0);
    }


}