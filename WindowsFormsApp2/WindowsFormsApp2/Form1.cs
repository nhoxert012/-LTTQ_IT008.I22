using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraScheduler;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private Timer timer;
        private TimeSpan ts;
        private int SliderValueOld;
        public Form1()
        {
            InitializeComponent();
        }

        //Đóng ứng dụng
        private void bunifuImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Phóng lớn màn hình
        private void bunifuImageButton3_Click(object sender, EventArgs e)
        {
            if(this.WindowState == FormWindowState.Maximized)
            {
                bunifuImageButton3.Image = Properties.Resources.icons8_Maximize_Window_50px;
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
                bunifuImageButton3.Image = Properties.Resources.icons8_Restore_Window_50px;
            }
        }
        
        //Thu nhỏ windown
        private void bunifuImageButton1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        
        //Thoát ứng dụng
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Mở file chơi nhạc
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.AddExtension = true;
            open.DefaultExt = "*.*";
            open.Filter = "Media(*.*)|*.*";
            open.ShowDialog();
            axWindowsMediaPlayer1.URL = open.FileName;
            axWindowsMediaPlayer1.settings.volume = trackBarControl1.Value;
        }

        //Play/Pause file nhạc đang chạy
        private void bunifuImageButton5_Click(object sender, EventArgs e)
        {
            Console.WriteLine(axWindowsMediaPlayer1.settings.mute);
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                bunifuImageButton5.Image = Properties.Resources.icons8_Pause_32px;
                //axWindowsMediaPlayer1.settings.volume = trackBarControl1.Value;
                //axWindowsMediaPlayer1.settings.mute = true;
            }
            else
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                bunifuImageButton5.Image = Properties.Resources.icons8_Play_32px;
                //axWindowsMediaPlayer1.settings.volume = trackBarControl1.Value;
                //axWindowsMediaPlayer1.settings.mute = true;
            }
        }

        //Tăng giảm âm thanh của Chương trình 
        private void trackBarControl1_EditValueChanged(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.volume = trackBarControl1.Value;
        }

        //Set timer khi Chương trình được load
        private void Form1_Load(object sender, EventArgs e)
        {
            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(timer_TickOne);
        }

        //Thay đổi giá trị trackbar, thời gian currenttime của file đang chạy 
        private void timer_TickOne(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                bunifuSlider1.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
                ts = TimeSpan.FromSeconds(bunifuSlider1.Value);
                if (ts.Hours > 0)
                {
                    TimeCount.Text = ts.ToString(@"hh\:mm\:ss");
                }
                else
                {
                    TimeCount.Text = ts.ToString(@"mm\:ss");
                }
            }
        }

        //Hiển thị tổng thời gian của audio, Maximum của trackbar audio, set timer với từng trạng thái audio
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            //media player control's playstate change event handler
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                bunifuSlider1.MaximumValue = (int)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                ts = TimeSpan.FromSeconds(bunifuSlider1.MaximumValue);
                if (ts.Hours > 0)
                {
                    TimeTotal.Text = ts.ToString(@"hh\:mm\:ss");
                }
                else{
                    TimeTotal.Text = ts.ToString(@"mm\:ss");
                }
                timer.Start();
                
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                timer.Stop();
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                timer.Stop();
                bunifuSlider1.Value = 0;
            }
        }

        //Tua audio theo trackbar audio
        private void bunifuSlider1_ValueChanged(object sender, EventArgs e)
        {
            //if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying && SliderValueOld != bunifuSlider1.Value)
            //{
            //    axWindowsMediaPlayer1.Ctlcontrols.currentPosition = bunifuSlider1.Value;
            //    Console.WriteLine(bunifuSlider1.Value);
            //}
            //SliderValueOld = bunifuSlider1.Value;
            
        }

        //Chơi file audio tại một vị trí của trackbar audio
        private void bunifuSlider1_ValueChangeComplete(object sender, EventArgs e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                axWindowsMediaPlayer1.Ctlcontrols.currentPosition = bunifuSlider1.Value;
            }
            //Console.WriteLine(bunifuSlider1.Value);

        }

        //Chức năng mute
        private void bunifuImageButton7_Click(object sender, EventArgs e)
        {
            if(axWindowsMediaPlayer1.settings.mute == false)
            {
                axWindowsMediaPlayer1.settings.mute = true;
                bunifuImageButton7.Image = Properties.Resources.icons8_mute_52;
            }
            else
            {
                axWindowsMediaPlayer1.settings.mute = false;
                bunifuImageButton7.Image = Properties.Resources.icons8_voice_48;
            }
        }
    }
}
