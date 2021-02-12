using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Text.RegularExpressions;
using System.Net;
using Shell32;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Security.Principal;
using Microsoft.Win32;
using HtmlAgilityPack;


namespace WindowsFormsApplication14
{

    public partial class fivelivesx : Form
    {
        decimal totalMemSize=0;
        // bool borderstyle = true;
        PowerStatus pwr = SystemInformation.PowerStatus;
        string uname;
        string basedrive;
        bool wireless = true;
        //   int timecount = 0;
        //    int checktime = 60;
        Image myimage = new Bitmap(@".\\.\\recycle_bin.png");
        Image myimage1 = new Bitmap(@".\\.\\recycle_bin_empty.png");
        bool firsttime = true;
        string[] lastones = new string[10];
        string[] lastdrives = new string[10];


        string currdrive = "";
        int counta = 0;
        string lastd = "";
        //    String xmlFile;
        String localxml;
        String xmllink;
        //    String xmlx;
        XmlDocument xml = new XmlDocument();
        string backgcolour;
        string incolour;
        string forecolour;
        string newspaperlink;
        string newspapertitle;
        string otherlink;
        string othertitle;
        string footielink;
        string footietitle;
        string tablelink;
        string tabletitle;
        string currtitle;
        string[] newspapers = new string[30];
        string[] others = new string[30];
        string[] footiex = new string[30];
        string[] tablex = new string[30];
        string[] currx = new string[20];
        string rootname;
        string systemdir;
        string directory;
        enum Day { Sunday, Monday, Tuesday, Wednesday };
        public fivelivesx()
        {
            int x = (int)Day.Wednesday;
            InitializeComponent();
            papers.Text = "";
            miscellaneous.Text = "";
            tables.Text = "";
            //check drives
            checkdrives();
            drivelist.DropDownStyle = ComboBoxStyle.DropDownList;
            driveinfo.DropDownStyle = ComboBoxStyle.DropDownList;
            Timer timex = new Timer();
            timex.Tick += new EventHandler(timex_Tick);
            timex.Interval = 1000;
            timex.Start();
            checkstatus();
            checkMem();
            get_xml_links();

            //set colours
            this.BackColor = System.Drawing.ColorTranslator.FromHtml(backgcolour);
            System.Drawing.Color setcolour = System.Drawing.ColorTranslator.FromHtml(incolour);

            dirlist.BackColor = this.BackColor;
            username.BackColor = this.BackColor;
            ipaddress.BackColor = this.BackColor;
            driveinfo.BackColor = this.BackColor;
            drivelist.BackColor = this.BackColor;
            googlein.BackColor = setcolour;
            excepterr.BackColor = this.BackColor;
            time.BackColor = this.BackColor;
            papers.BackColor = this.BackColor;
            miscellaneous.BackColor = this.BackColor;
            memoryData.BackColor = this.BackColor;
            footie.BackColor = this.BackColor;
            tables.BackColor = this.BackColor;
            currency.BackColor = setcolour;
            fromcur.BackColor = this.BackColor;
            tocur.BackColor = this.BackColor;
            powerdetails.BackColor = this.BackColor;
            driveinfo.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            ipaddress.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            username.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            drivelist.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            papers.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            miscellaneous.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            footie.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            tables.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            tables.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            currency.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            fromcur.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            tocur.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            time.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            dirlist.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);
            googlein.ForeColor = System.Drawing.ColorTranslator.FromHtml(forecolour);

            //tocur.SelectedIndexChanged += (xe_Click_1);
            //fromcur.SelectedIndexChanged += (xe_Click_1);
            miscellaneous.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
            papers.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
            tables.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
            footie.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
            dirlist.MouseWheel += new MouseEventHandler(comboBox1_MouseWheel);
            papers.SelectedIndexChanged += (go_Click);
            footie.SelectedIndexChanged += (gofoot_Click);
            tables.SelectedIndexChanged += (gotabs_Click);
            miscellaneous.SelectedIndexChanged += (gomisc_Click);

            papers.DropDownClosed += (papers_MouseLeave);
            this.drivelist.SelectedIndexChanged +=
            new System.EventHandler(drivelist_SelectedIndexChanged);
            drivelist_SelectedIndexChanged(null, null);
        }




        private void timex_Tick(object sender, EventArgs e)
        {
            if(pwr.PowerLineStatus == PowerLineStatus.Offline)
                    powerdetails.Text = "Not plugged in ";
            else
                    powerdetails.Text = "Plugged in ";
            if (pwr.BatteryLifePercent < 0.15)
                powerdetails.ForeColor = Color.Red;
            else
                powerdetails.ForeColor = Color.Black;

            powerdetails.Text += pwr.BatteryLifePercent.ToString("P0");

            DateTime curTime = DateTime.Now;
            string suffix = "th ";
            int dayIs = Convert.ToInt32(DateTime.Now.ToString("dd"));

            if (dayIs % 10 == 1)
                suffix = "st ";
            if (dayIs % 10 == 2)
                suffix = "nd ";
            if (dayIs % 10 == 3 && dayIs != 13)
                suffix = "rd ";

            time.Text = dayIs + suffix + DateTime.Now.ToString("MMM yyyy  HH:mm:ss");
             if (!wireless & curTime.Minute % 10 == 0 & curTime.Second == 0)
             {
              //   timecount = 0;
                 checkstatus();
                //xe_Click_1(null, null);             
             }

              if (wireless & curTime.Minute % 5 == 0 & curTime.Second == 0)
              {
              //   timecount = 0;
                 checkstatus();
                //xe_Click_1(null, null);
              }

            if (wireless & curTime.Second % 15 == 0)
            {
                checkMem();
            }
        }

        private void checkMem()
        {
            ManagementObjectSearcher os_searcher =
    new ManagementObjectSearcher(
        "SELECT * FROM Win32_OperatingSystem");

            memoryData.Text = "";
            foreach (ManagementObject mobj in os_searcher.Get())
            {
                totalMemSize = 0;
                GetInfo(mobj, "TotalVisibleMemorySize");
                GetInfo(mobj, "FreePhysicalMemory");
                //GetInfo(mobj, "FreeSpaceInPagingFiles");
                //GetInfo(mobj, "FreeVirtualMemory");
                //GetInfo(mobj, "SizeStoredInPagingFiles");
                //GetInfo(mobj, "TotalSwapSpaceSize");
                //GetInfo(mobj, "TotalVirtualMemorySize");
            }
        }
        private void GetInfo(ManagementObject mobj, string property_name)
        {
            object property_obj = mobj[property_name];
            if (property_obj == null)
            {
              //  memoryData.Text= property_name + "???";
            }
            else
            {

                int getGb = 1024*1024;
                ulong memSize = (ulong)property_obj;
                decimal property_value = (decimal)memSize / (decimal)getGb;
                memoryData.Text += property_name +" "+ property_value.ToString("0.####")+" GB\n";
                if (totalMemSize == 0)
                    totalMemSize = property_value;
                if (totalMemSize - property_value > 0)
                {
                    memoryData.Text += "Memory Usage " + (totalMemSize - property_value).ToString("0.####") + " GB ";
                    memoryData.Text += ((totalMemSize - property_value) * 100 / totalMemSize).ToString("0.##") + "%";
                }
            }
        }


        private void time_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Date and Time";
        }


       private void button1_Click(object sender, EventArgs e)
        {

           string website = "http://www.guardian.co.uk/";
           runproc(website, "The Guardian");


        }
       private void button1_MouseHover(object sender, EventArgs e)
       {
           excepterr.Text = "The Guardian";
       }


       private void button2_Click(object sender, EventArgs e)
       {

           string website = "http://mail.yahoo.com";
           runproc(website, "Yahoo Mail");

       }
       private void button2_MouseHover(object sender, EventArgs e)
       {
           excepterr.Text = "Yahoo Mail";
       }


       private void bbcnews_Click(object sender, EventArgs e)
       {

           string website = "http://www.bbc.co.uk/news";
           runproc(website, "BBC News");

       }
       private void bbcnews_MouseHover(object sender, EventArgs e)
       {
           excepterr.Text = "BBC News";
       }


       private void bbcsport_Click(object sender, EventArgs e)
       {

           string website = "http://www.bbc.co.uk/sport";
           runproc(website, "BBC Sport");

       }
       private void bbcsport_MouseHover(object sender, EventArgs e)
       {
           excepterr.Text = "BBC Sport";
       }


       private void slingbox_Click(object sender, EventArgs e)
       {
           string website = "\"C:\\Program Files (x86)\\Slingplayer Desktop\\Slingplayer Desktop.exe\"";
               runproc(website, "Slingplayer");

       }
       private void slingbox_MouseHover(object sender, EventArgs e)
       {
           excepterr.Text = "Slingplayer";
       }


       private void outlook_Click(object sender, EventArgs e)
       {
           string website = "\"C:\\Program Files\\Microsoft Office\\Office14\\outlook.exe\"";
           runproc(website, "Outlook");
       }
       private void outlook_MouseHover(object sender, EventArgs e)
       {
           excepterr.Text = "Outlook";
       }


       private void xchange_Click(object sender, EventArgs e)
       {
           string website = "\".\\.\\CURRENCYX.exe\"";
           runproc(website, "Currency");

       }
       private void xchange_MouseHover(object sender, EventArgs e)
       {
           excepterr.Text = "Currency";
       }


       private void explorer_Click(object sender, EventArgs e)
       {
           string website = "\"explorer.exe\"";
           runproc(website, "Explorer");
           
       }

        private void vstudio_Click(object sender, EventArgs e)
        {
            string website = @"C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Visual Studio.lnk";
            runproc(website, "Visual Studio");

        }
        private void vstudio_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Visual Studio";
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void skype_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files (x86)\\Skype\\Phone\\skype.exe\"";
            runproc(website, "Skype");

        }
        private void skype_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Skype";
        }

        private void taskmgr_Click(object sender, EventArgs e)
        {
            string website = "taskmgr.exe";
            runproc(website, "Task Manager");

        }
        private void taskmgr_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Task Manager";
        }
        
        private void google_Click(object sender, EventArgs e)
        {
            String testmatch = googlein.Text;

            testmatch = testregex(testmatch);

      

            string website = "\"http://www.google.co.uk/search?hl=en&ie=UTF-8&oe=UTF-8&q=" + testmatch + "&btnG=Google\"";
             runproc(website, "Google");

        }
        private void google_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Google";
        }

        private void clear_Click(object sender, EventArgs e)
        {
            googlein.Text = "";
 
        }

        private void googlenews_Click(object sender, EventArgs e)
        {
            String testmatch = googlein.Text;

            testmatch = testregex(testmatch);

            string website = "\"http://news.google.com/nwshp?hl=en&tab=nn&q=" + testmatch + "\"";
            runproc(website, "Google News");

        }
        private void googlenews_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Google News";
        }

        private void vmware_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files (x86)\\VMware\\VMware Workstation\\vmware.exe\"";
            runproc(website, "VMWare");

        }

        private void vmware_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "VMWare";
        }


        private void wiki_Click(object sender, EventArgs e)
        {
            String testmatch = googlein.Text;

            testmatch = testregex(testmatch);

            ///       string website = "\"http://en.wikipedia.org/wiki/" + wikipedia.Text + "\"";
            string website = "\"http://en.wikipedia.org/wiki/" + testmatch + "\"";
               runproc(website, "Wikipedia");

        }
        private void wiki_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Wikipedia";
        }

        private void services_Click(object sender, EventArgs e)
        {
            string website = "services.msc";
            runproc(website, "Services");
        }
        private void services_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Services";
        }

        private void itune_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files\\iTunes\\iTunes.exe\"";
            runproc(website, "iTunes");

        }
        private void itune_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "iTunes";
        }

        private void runproc(string pathrun, string desc)
        {
            excepterr.Text = desc;
            try
            {
                System.Diagnostics.Process.Start(pathrun);
            }
            catch (Exception)
            {
                excepterr.Text = desc + ", error locating file or website";
            }
        }


        public string testregex(string testmatch)
        {
                        String hashmatch = "(\\x23)+";
            String ampmatch = "(\\x26)+";
            String plusmatch = "(\\x2B)+";
            Match match = Regex.Match(testmatch, @hashmatch,
    RegexOptions.IgnoreCase);

            Match match1 = Regex.Match(testmatch, @ampmatch,
RegexOptions.IgnoreCase);

            Match match2 = Regex.Match(testmatch, @plusmatch,
RegexOptions.IgnoreCase);

            // Here we check the Match instance.
            if (match.Success)
            {
                // Finally, we get the Group value and display it.
                string key = match.Groups[1].Value;
                string result = Regex.Replace(testmatch, "#", "%23");
                testmatch = result;
            }

            if (match1.Success)
            {
                // Finally, we get the Group value and display it.
                string key = match1.Groups[1].Value;
                string result = Regex.Replace(testmatch, "&", "%26");
                testmatch = result;
            }


            if (match2.Success)
            {
                // Finally, we get the Group value and display it.
                string key = match2.Groups[1].Value;
                string result = Regex.Replace(testmatch, "\\x2b", "%2b");
                testmatch = result;
            }
            return testmatch;
        }

        private void excepterr_TextChanged(object sender, EventArgs e)
        {

        }

        private void notepad_Click(object sender, EventArgs e)
        {
            string website = "notepad.exe";
            runproc(website, "Notepad");

        }

        private void notepad_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Notepad";
        }


        private void homepage_Click(object sender, EventArgs e)
        {
            string website = "http://zhongwenweb.com//misc.htm";
            runproc(website, "my homepage");

        }

 
        private void homepage_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "my homepage";
        }

        private void firefox_Click(object sender, EventArgs e)
        {
            string website = "%programfiles(x86)%\\Mozilla Firefox\\firefox.exe";
            runproc(website, "Firefox");

        }

        private void firefox_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "firefox";
        }

        private void chrome_Click(object sender, EventArgs e)
        {
            string path = "%programfiles(x86)%\\Google\\Chrome\\Application\\chrome.exe";
            string website = Environment.ExpandEnvironmentVariables(path);

             runproc(website, "Google Chrome");


        }
        private void chrome_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Google Chrome";
        }

        private void facebook_Click(object sender, EventArgs e)
        {
            string website = "http://facebook.com";
            runproc(website, "Facebook");


        }
        private void facebook_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Facebook";
        }

        private void go_Click(object sender, EventArgs e)
        {
            int indexref;

        {

                indexref = Convert.ToInt32(papers.SelectedIndex);

         //       string website = newspapers[indexref];
                string website = Environment.ExpandEnvironmentVariables(newspapers[indexref]);
                runproc(website, papers.Text);
        }




        }

        private void gomisc_Click(object sender, EventArgs e)
        {
            int indexref;

            {

                indexref = Convert.ToInt32(miscellaneous.SelectedIndex);

            //    string website = others[indexref];
                string website = Environment.ExpandEnvironmentVariables(others[indexref]);
                runproc(website, miscellaneous.Text);
            }



        }

        private void gofoot_Click(object sender, EventArgs e)
        {

            int indexref;

            indexref = Convert.ToInt32(footie.SelectedIndex);

        //    string website = footiex[indexref];
            string website = Environment.ExpandEnvironmentVariables(footiex[indexref]);
            runproc(website, footie.Text);

        }

        private void gotabs_Click(object sender, EventArgs e)
        {
            int indexref;

            indexref = Convert.ToInt32(tables.SelectedIndex);

       //     string website = tablex[indexref];
            string website = Environment.ExpandEnvironmentVariables(tablex[indexref]);
            runproc(website, tables.Text);

        }

        private void football_Click(object sender, EventArgs e)
        {
            string website = ".\\.\\football.exe";
            runproc(website, "RSS Feeds");

        }
        private void football_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "RSS Feeds";
        }

        private void clock_Click(object sender, EventArgs e)
        {
            string website = ".\\.\\clock.exe";
            runproc(website, "Clock");

        }
        private void clock_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Clock";
        }

        private void stopwatch_Click(object sender, EventArgs e)
        {
            string website = ".\\.\\stopwatch.exe";
            runproc(website, "Stopwatch");

        }

        private void stopwatch_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Stopwatch";
        }
 
        private void teamviewer_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files (x86)\\TeamViewer\\Version7\\TeamViewer.exe\"";
            runproc(website, "Teamviewer");


        }
        private void teamviewer_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Teamviewer";
        }

        private void iPlayerone_Click(object sender, EventArgs e)
        {

            string website = "http://www.bbc.co.uk/iplayer/live/bbcone";

                runproc(website, "BBC One iPlayer");

        }

        private void iPlayerone_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC One iPlayer";
        }

        private void iPlayer2_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/iplayer/live/bbctwo";
            runproc(website, "BBC Two iPlayer");

 
        }
        private void iPlayer2_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC Two iPlayer";
        }

        private void iPlayer4_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/iplayer/live/bbcfour";
            runproc(website, "BBC Four iPlayer");

        }
        private void iPlayer4_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC Four iPlayer";
        }

        private void mspaint_Click(object sender, EventArgs e)
        {
            string website = "mspaint.exe";
            runproc(website, "MSPAINT");

        }


        private void mspaint_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "MSPAINT";
        }

        private void iPlayernews_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/iplayer/live/bbcnews";
            runproc(website, "BBC News iPlayer");

        }

        private void iPlayernews_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC News iPlayer";
        }

        private void iPlayer_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/iplayer/";
            runproc(website, "BBC iPlayer");

        }


        private void iPlayer_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC iPlayer";
        }

        private void ipaddress_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "IP Address";
        }

        private void diskspace_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Disk Space Max, Free";
        }


        private void schedule1_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/iplayer/schedules/bbcone";
            runproc(website, "BBC One Schedule");

        }
        private void schedule1_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC One Schedule";
        }

        private void schedule2_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/iplayer/schedules/bbctwo";
            runproc(website, "BBC Two Schedule");

        }
        private void schedule2_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC Two Schedule";
        }

        private void schedule4_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/iplayer/schedules/bbcfour";
            runproc(website, "BBC Four Schedule");

        }
        private void schedule4_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC Four Schedule";
        }

        private void schedulenews_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/iplayer/schedules/bbcnews";
            runproc(website, "BBC News Schedule");

        }
        private void schedulenews_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC News Schedule";
        }


        private void googlemap_Click(object sender, EventArgs e)
        {
            String testmatch = googlein.Text;

            testmatch = testregex(testmatch);

            string website = "\"http://www.google.co.uk/maps/place/" + testmatch + "\"";
            runproc(website, "Google Map");
        }
        private void googlemap_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Google Map";
        }

        private void rightmove_Click(object sender, EventArgs e)
        {
            String testmatch = googlein.Text;

            testmatch = testregex(testmatch);

            string website = "\"http://www.rightmove.co.uk/property-for-sale/search.html?searchLocation=" + testmatch + "&locationIdentifier=&useLocationIdentifier=false&buy=For+sale" + "\"";
            runproc(website, "Rightmove");

        }

        private void rightmove_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Rightmove";
        }

        private void twitter_Click(object sender, EventArgs e)
        {
            String testmatch = googlein.Text;

            testmatch = testregex(testmatch);

            string website = "\"https://twitter.com/search?q=" + testmatch + "\"";
            runproc(website, "Twitter");
        }


        private void twitter_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Twitter";
        }

        private void youtube_Click(object sender, EventArgs e)
        {
              String testmatch = googlein.Text;

            testmatch = testregex(testmatch);

            string website = "\"http://www.youtube.com/results?search_query=" + testmatch + "\"";
            runproc(website, "YouTube");          
        }

        private void youtube_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "YouTube";
        }

        private void word_Click(object sender, EventArgs e)
        {
           string website = "\"C:\\Program Files\\Microsoft Office\\Office14\\winword.exe\"";
           runproc(website, "Word");
       }
        private void word_MouseHover(object sender, EventArgs e)
       {
           excepterr.Text = "Word";
       }

        private void powerpoint_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files\\Microsoft Office\\Office14\\powerpnt.exe\"";
            runproc(website, "Powerpoint");

        }
        private void powerpoint_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Powerpoint";
        }

        private void excel_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files\\Microsoft Office\\Office14\\excel.exe\"";
            runproc(website, "Excel");

        }

        private void excel_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Excel";
        }

        private void visio_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files\\Microsoft Office\\Office14\\visio.exe\"";
            runproc(website, "Visio");

        }
        private void visio_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Visio";
        }

        private void googlein_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                  googlein.ContextMenuStrip = contextMenuStrip1;
                  var menuText = contextMenuStrip1.Text;

 
            }
        }





        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            googlein.Paste();

        }



        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            googlein.Copy();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            googlein.Undo();

        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            googlein.Cut();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            googlein.SelectAll();
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            googlein.Clear();

        }

        private void get_iplayer_Click(object sender, EventArgs e)
        {
            //     string website = "\".\\.\\g_i.bat\"";
            string website = ".\\.\\getiplayer.exe";
            runproc(website, "get_iplayer");

        }
        private void get_iplayer_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "get_iplayer";
        }

        private void acdsee_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files (x86)\\ACD Systems\\ACDSee\\12.0\\ACDSee12.exe\"";
            string pathis = "\"" + dirlist.Text;
            System.Diagnostics.Process.Start(website, pathis);

         //   runproc(website, "ACDSee");

        }
                private void acdsee_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "ACDSee";
        }

        private void nhk_Click(object sender, EventArgs e)
        {
            string website = "http://www3.nhk.or.jp//nhkworld//w//movie";
            runproc(website, "NHK World");
 
        }
        private void nhk_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "NHK World";
        }

        private void hsbc_Click(object sender, EventArgs e)
        {
            string website = "http://www.hsbc.com.hk/";
            runproc(website, "HSBC Hong Kong");

        }

        private void hsbc_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "HSBC Hong Kong";
        }

        private void username_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Computer Name\\User Name";
        }

        private void volume_Click(object sender, EventArgs e)
        {
            string website = "SndVol.exe";
            runproc(website, "Volume Control");

        }
        private void volume_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Volume Control";
        }

        private void downloads_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Users\\"+uname+"\\AppData\\Local\\BBC\\BBC iPlayer Downloads\\BBC iPlayer Downloads.exe\"";
            runproc(website, "iPlayer Downloads");

        }

        private void downloads_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "iPlayer Downloads";
        }

        private void printer_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files\\Canon\\MyPrinter\\BJMyPrt.exe\"";
            runproc(website, "Canon Printer");
        }
        private void printer_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Canon Printer";
        }

        private void googlein_enter(object sender, KeyPressEventArgs e)
        {
            String testmatch = googlein.Text;
            testmatch = testregex(testmatch);

            if (e.KeyChar == (char)13)
            {
                e.Handled = true;
                googlein.Clear();
                testmatch = testmatch.Replace("\n", String.Empty);
                googlein.Text = testmatch;
                googlein.Select(googlein.Text.Length, 0);

                string website = "\"http://www.google.co.uk/search?hl=en&ie=UTF-8&oe=UTF-8&q=" + testmatch + "&btnG=Google\"";
                runproc(website, "Google");
            }
        }




        //private void xe_Click_1(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string website = "http://www.xe.com/currencyconverter/convert/?Amount=" + currency.Text + "&From=" + fromcur.Text + "&To=" + tocur.Text + " v&r=#rates/";

        //        HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

        //        // There are various options, set as needed
        //        var url = website;
        //        var web = new HtmlWeb();
 
        //        var doc = web.Load(url);
 
        //        if (doc.DocumentNode != null)
        //        {
        //            //     HtmlAgilityPack.HtmlNode titleNode = doc.DocumentNode.SelectSingleNode("//uccResultUnit");

        //            foreach (HtmlAgilityPack.HtmlNode titleNode in doc.DocumentNode.SelectNodes("//div"))
        //            {
        //            ///This is the table.
        //            if(titleNode.Id == "reactContainer")
        //    //        foreach (HtmlAgilityPack.HtmlNode row in titleNode.SelectNodes("class"))
        //            {

        //                string responseis = titleNode.InnerText;

        //                var cur1 = fromcur.Text + " =";
        //                var cur2 = tocur.Text;
        //                int pFrom = responseis.IndexOf(cur1) + cur1.Length;
        //                int pTo = responseis.IndexOf(cur2) + cur2.Length;
        //                for (int i = 1; i < 3; i++)
        //                {
        //                    if (pTo - pFrom - 3 < 0)
        //                    {
        //                        pTo = responseis.IndexOf(cur2, pTo + 1);
        //                    }
        //                }
        //                String result = responseis.Substring(pFrom, pTo - pFrom -3);
        //                DateTime curTime = DateTime.Now;
        //                }
        //            }
        //    }
        //}

        //    catch (Exception)
        //    {
        //    }


        //    //      runproc(website, "XE");

        //}

        private void xe_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "XE Currency";
        }

        private void currency_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                currency.ContextMenuStrip = contextMenuStrip2;
                var menuText = contextMenuStrip2.Text;


            }
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            currency.Clear();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            currency.Clear();
            currency.Text = "1";

        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            currency.Copy();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            currency.SelectAll();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            currency.Paste();
        }

        private void gotoolStripMenuItem7_Click(object sender, EventArgs e)
        {

            String testmatch = googlein.Text;

            testmatch = testregex(testmatch);



            string website = "\"http://www.google.co.uk/search?hl=en&ie=UTF-8&oe=UTF-8&q=" + testmatch + "&btnG=Google\"";
            runproc(website, "Google");

        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            string website = "http://www.xe.com/currencyconverter/convert/?Amount=" + currency.Text + "&From=" + fromcur.Text + "&To=" + tocur.Text + " v&r=#rates/";
            runproc(website, "XE");

        }



        private void rectangleShape2_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/iplayer/";

            runproc(website, "BBC iPlayer");

        }

        private void rectangleShape1_Click(object sender, EventArgs e)
        {
            string website = "http://www.xe.com/currencyconverter/convert/?Amount=" + currency.Text + "&From=" + fromcur.Text + "&To=" + tocur.Text + " v&r=#rates/";
            runproc(website, "XE");

        }

        private void youtube2mp3_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files (x86)\\DVDVideoSoft\\Free YouTube to MP3 Converter\\FreeYouTubeToMP3Converter.exe\"";
            runproc(website, "Youtube to MP3");

        }
        private void youtube2mp3_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Youtube to MP3";
        }

        private void youtubedown_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files (x86)\\DVDVideoSoft\\Free YouTube Download\\FreeYTVDownloader.exe\"";
            runproc(website, "Youtube Download");

        }

        private void youtubedown_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Youtube Download";
        }



        private void linkedin_Click(object sender, EventArgs e)
        {
            {
                string website = "https://www.linkedin.com/";
                runproc(website, "Linkedin");

            }
        }
        private void linkedin_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Linkedin";
        }

        private void rectangleShape3_Click(object sender, EventArgs e)
        {

        }

        private void rectangleShape4_Click(object sender, EventArgs e)
        {

        }

        private void moviemaker_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files (x86)\\Windows Live\\Photo Gallery\\MovieMaker.exe\"";
            runproc(website, "Movie Maker");

        }
        private void moviemaker_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Movie Maker";
        }

        private void get_iplayerlist_Click(object sender, EventArgs e)
        {
          //  string website = "notepad list.txt";
            Process myProcess = null;
            myProcess = System.Diagnostics.Process.Start(".\\.\\doit.bat ");
            // runproc(website, "get_iplayer List");
            myProcess.WaitForExit();
            if (myProcess.HasExited)
            {
             //   runproc(website, "get_iplayer List");
                Process.Start("notepad.exe", "list.txt");
            }
        }
        private void get_iplayerlist_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "get_iplayer List";
        }

        private void pgce_Click(object sender, EventArgs e)
        {
            string website = "\".\\.\\pgce.exe\"";
            runproc(website, "PGCE");

        }
        private void pgce_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "PGCE";
        }

        private void checkstatus()
        {
            IPHostEntry host;

            String LocalHostName;
            try
            {
                LocalHostName = Dns.GetHostName();
                host = Dns.GetHostEntry(LocalHostName);
                ipaddress.Text = host.AddressList[1].ToString();
                if (ipaddress.Text == "127.0.0.1")
                {
                    ipaddress.Text = "No wireless";
                }
                int countera = 0;
                ipaddress.Text = host.AddressList[countera].ToString();
                while(ipaddress.Text[0].ToString() == "f")
        //        if (ipaddress.Text[0].ToString() == "f")
                {
                    ipaddress.Text = host.AddressList[countera+1].ToString();
                    countera++;
                }

                wireless = true;
             //   checktime = 60;

            }
            catch (Exception)
            {
                ipaddress.Text = "No Wireless";
                wireless = false;
              //  checktime = 10;
           }

            checkdrives();
            drivelist.DropDownStyle = ComboBoxStyle.DropDownList;
            driveinfo.DropDownStyle = ComboBoxStyle.DropDownList;
            String iUserName;
            //  iUserName = Environment.UserName;
            iUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            username.Text = iUserName;

            uname = Environment.UserName;

          //  this.Click += new System.EventHandler(form1_pressed);


            // check if recycle bin is empty or not

            Shell shell = new Shell();

           recycle.BackgroundImage = myimage;
           Folder recycleBin = shell.NameSpace(10);
           int itemsCount = recycleBin.Items().Count;

            if (itemsCount == 0)
            {

              recycle.BackgroundImage = myimage1;
            }

        }

        private void explorer_Click_1(object sender, EventArgs e)
        {
            directory = dirlist.Text;
            try
            {
                System.Diagnostics.Process.Start(directory, "");
            }
            catch
            {
                dirlist.Text = "no directory";
            }
        }


        private void explorer_MouseHover(object sender, EventArgs e)
        {

            excepterr.Text = "Explorer";
        }

     private void run_Click(object sender, EventArgs e)
        {
            string website = "run";
            runproc(website, "run");
        }


        private void access_Click(object sender, EventArgs e)
        {
            string website = "\"C:\\Program Files\\Microsoft Office\\Office14\\msaccess.exe\"";
            runproc(website, "Access");
        }
        private void access_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Access";
        }

        private void lwa_Click(object sender, EventArgs e)
        {
            string website = "https://vpx.leedswestacademy.org.uk/vpn/index.html";
            runproc(website, "LWA Remote Access");

        }
        private void lwa_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "LWA Remote Access";
        }

        private void hiragana_Click(object sender, EventArgs e)
        {
            string website = "\".\\.\\hiragana.exe\"";
            runproc(website, "Hiragana");
        }
        private void hiragana_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Hiragana";
        }

        private void bbc5live_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/radio/player/bbc_radio_five_live";
            runproc(website, "BBC 5 Live");

        }
        private void bbc5live_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC 5 Live";
        }

        private void radio4_Click(object sender, EventArgs e)
        {

            string website = "http://www.bbc.co.uk/radio/player/bbc_radio_fourfm";
            runproc(website, "BBC Radio 4");
        }
        private void radio4_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC Radio 4";
        }

        private void ws_Click(object sender, EventArgs e)
        {

            string website = "http://www.bbc.co.uk/radio/player/bbc_world_service";
            runproc(website, "BBC World Service");
        }
        private void ws_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC World Service";
        }

        private void refresh_Click(object sender, EventArgs e)
        {
            checkstatus();
        }

        private void rio2016_Click(object sender, EventArgs e)
        {
            string website = "https://www.rio2016.com/en";
            runproc(website, "Rio 2016 Olympics");
        }
        private void rio2016_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Rio 2016 Olympics";
        }

        private void recycle_Click(object sender, EventArgs e)
        {

        // Do not show confirm message box
        const int SHERB_NOCONFIRMATION = 0x00000001;
         
        // empty Recycle Bin
        SHEmptyRecycleBin(IntPtr.Zero, null, SHERB_NOCONFIRMATION);

         }
             [DllImport("shell32.dll")]
               static extern int SHEmptyRecycleBin(IntPtr hWnd, string pszRootPath, uint dwFlags);
          
     
 

        private void recycle_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Recycle Bin";
        }

        private void cmd_Click(object sender, EventArgs e)
        {

            System.Diagnostics.Process.Start("cmd.exe", "/k cd /d " +  dirlist.Text);

        }

        private void cmd_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Command Line";
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
           // direct.Clear();
            drivelist.Text = basedrive;
 
        }

        private void checkdrives()
        {


            DriveInfo[] allDrives = DriveInfo.GetDrives();

            for (int counter=0; counter < counta; counter++)
            {
                drivelist.Items.Remove(lastdrives[counter]);
                driveinfo.Items.Remove(lastones[counter]);
            }
            counta = 0;
            String outstring = "#,#0.####";
            double gigabyte = 1024 * 1024 * 1024;
          //  drivelist.Text = "";
         //   driveinfo.Text = "";
            foreach (DriveInfo d in allDrives)
            {
                if (counta == 0)
                {
               //     drivelist.Text = d.Name;
               //      currdrive = drivelist.Text;
                    basedrive = d.Name;

              //      currdrive = basedrive;
                }
                if (d.IsReady == true)
                {
                    drivelist.Items.Add(d.Name);
                    string disktext;
                    string drivex = d.Name;
                    string currentdrive = drivex.Substring(0, 2) + '"';
                    string curdrive = drivex + '"';
                    currdrive = drivex;
                    // drive info
                    string driveid = "win32_logicaldisk.deviceid=\"" + curdrive;
                    string newdriveid = "win32_logicaldisk.deviceid=\"" + currentdrive;
                    ManagementObject disk = new ManagementObject(newdriveid);
                    
                    disk.Get();
                    string disksize = "";
                    disksize = disk["Size"].ToString();
                    string diskfree = "";
                    diskfree = disk["FreeSpace"].ToString();
                    double diskc = Convert.ToDouble(disksize);
                    int diskh = disksize.Length;
                    int diskx = diskfree.Length;
                    int diskz = diskh - diskx;
                    diskc = diskc / gigabyte;
                    double diskSizeIs = diskc;

                    disktext = (drivex + " Size = " + diskc.ToString(outstring) + " Gbytes   ");
                    diskc = Convert.ToDouble(diskfree);
                    diskc = diskc / gigabyte;

                    disktext += ("Free = ");
                    for (int x = 0; x <= diskz; x++)
                    {
                        disktext += " ";
                    }

                    disktext += diskc.ToString(outstring) + " Gbytes";

                    if (diskSizeIs > 0)
                        disktext += " (" + (diskc * 100 / diskSizeIs).ToString("#0.##") + "%)";
                        

                    if (firsttime==false)
                    {
                    }
                    driveinfo.Items.Add(disktext);
                    if (!firsttime)
                    {
                        drivelist.Text = lastd;
                    }
                    if (counta == 0)
                    {
                    //    driveinfo.Text = "";
                   //     drivelist.Text = "";
                          driveinfo.Text = disktext;
                   //     drivelist.Text = d.Name;
                     }
                    if (firsttime)
                    {
                        drivelist.Text = basedrive;
   //                     driveinfo.Text = currinfo;
   //                     currinfo = driveinfo.Text;
                    }
                    lastones[counta] = disktext;
                    lastdrives[counta] = d.Name;
                    counta++;
                }

            }
            if (counta > 0)
            {
                firsttime = false;
                lastd = basedrive;
    //            drivelist.Text = currdrive;
  //              driveinfo.Text = currinfo;
            }

        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
      //      direct.Paste();

        }


        private void direct_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Directory";
        }

        private void drivelist_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Drives";
        }

        private void driveinfo_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Drive Infomation";
        }

        private void bbc5livesx_Click(object sender, EventArgs e)
        {
            string website = "http://www.bbc.co.uk/radio/player/bbc_radio_five_live_sports_extra";
            runproc(website, "BBC 5 Live Sports Extra");
        }

        private void bbc5livesx_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "BBC 5 Live Sports Extra";
        }

        private void clear_Click_1(object sender, EventArgs e)
        {
            googlein.Clear();
        }

        private void paste_Click(object sender, EventArgs e)
        {
            googlein.Paste();
        }

        private void get_xml_links()
        {
            backgcolour = "";
            incolour = "";
            forecolour = "";
            newspaperlink = "";
            newspapertitle = "";
            currtitle = "";

            //xml stuff
            int countit = 0;

            localxml = ".\\.\\stuff.xml";
            if (!File.Exists(localxml))
            {
                excepterr.Text = "No XML file exists";
          //      go.Enabled = false;
          //      gomisc.Enabled = false;
          //      gofoot.Enabled = false;
          //      gotabs.Enabled = false;
            }
            else
            {
                xml.Load(localxml);
                XmlNodeList xnColours = xml.SelectNodes("/values");
                foreach (XmlNode xn in xnColours)
                {
                    backgcolour = xn["backcolour"].InnerText;
                    incolour = xn["incolour"].InnerText;
                    forecolour = xn["foreground"].InnerText;
                }
                xml.Load(localxml);
                XmlNodeList xnnewspapers = xml.SelectNodes("/values/newspapers");
                countit = 0;
                if (xnnewspapers.Count > 0)
                {
                    foreach (XmlNode xnx in xnnewspapers)
                    {
                        newspapertitle = xnx["title"].InnerText;
                        newspaperlink = xnx["link"].InnerText;
                        //     xmlx = Convert.ToInt32(xmllink);
                        papers.Items.Add(newspapertitle);
                        if (countit == 0)
                        {
                            papers.Text = newspapertitle;
                        }
                        //    papers.Items.AddRange (new string[] { newspapertitle, newspaperlink });
                        //  indexit = Convert.ToInt32(papers.SelectedItem);
                        newspapers[countit] = newspaperlink;
                        countit++;
                    }
                    papers.DropDownStyle = ComboBoxStyle.DropDownList;
                }
        //        else
        //        {
       //             go.Enabled = false;
        //        }

                xml.Load(localxml);
                XmlNodeList xnothers = xml.SelectNodes("/values/others");
                countit = 0;
                if (xnothers.Count > 0)
                {
                    foreach (XmlNode xny in xnothers)
                    {
                        othertitle = xny["title"].InnerText;
                        otherlink = xny["link"].InnerText;
                        //     xmlx = Convert.ToInt32(xmllink);
                        miscellaneous.Items.Add(othertitle);
                        if (countit == 0)
                        {
                            miscellaneous.Text = othertitle;
                        }
                        //    papers.Items.AddRange (new string[] { newspapertitle, newspaperlink });
                        //  indexit = Convert.ToInt32(papers.SelectedItem);
                        others[countit] = otherlink;
                        countit++;

                    }
                    miscellaneous.DropDownStyle = ComboBoxStyle.DropDownList;
                }
        //        else
        //        {
        //            gomisc.Enabled = false;
       //         }
                xml.Load(localxml);
                XmlNodeList xnfootie = xml.SelectNodes("/values/footie");
                countit = 0;
                if (xnfootie.Count > 0)
                {
                    foreach (XmlNode xnz in xnfootie)
                    {
                        footietitle = xnz["title"].InnerText;
                        footielink = xnz["link"].InnerText;
                        //     xmlx = Convert.ToInt32(xmllink);
                        footie.Items.Add(footietitle);
                        if (countit == 0)
                        {
                            footie.Text = footietitle;
                        }
                        //    papers.Items.AddRange (new string[] { newspapertitle, newspaperlink });
                        //  indexit = Convert.ToInt32(papers.SelectedItem);
                        footiex[countit] = footielink;
                        countit++;

                    }
                    footie.DropDownStyle = ComboBoxStyle.DropDownList;

                }
       //         else
       //         {
       //             gofoot.Enabled = false;
        //        }


                xml.Load(localxml);
                XmlNodeList xntables = xml.SelectNodes("/values/tables");
                countit = 0;
                if (xntables.Count > 0)
                {
                    foreach (XmlNode xna in xntables)
                {
                    tabletitle = xna["title"].InnerText;
                    tablelink = xna["link"].InnerText;
                    //     xmlx = Convert.ToInt32(xmllink);
                    tables.Items.Add(tabletitle);
                    if (countit == 0)
                    {
                        tables.Text = tabletitle;
                    }
                    //    papers.Items.AddRange (new string[] { newspapertitle, newspaperlink });
                    //  indexit = Convert.ToInt32(papers.SelectedItem);
                    tablex[countit] = tablelink;
                    countit++;

                }
                    tables.DropDownStyle = ComboBoxStyle.DropDownList;
                }
      //          else
     //           {
      //              gotabs.Enabled = false;
       //         }

                xml.Load(localxml);
                XmlNodeList xncurr = xml.SelectNodes("/values/xe");
                countit = 0;
                foreach (XmlNode xnb in xncurr)
                {
                    currtitle = xnb["curr"].InnerText;
                    //     xmlx = Convert.ToInt32(xmllink);
                    fromcur.Items.Add(currtitle);
                    tocur.Items.Add(currtitle);
                    if (countit == 0)
                    {
                        fromcur.Text = currtitle;
                    }
                    if (countit == 1)
                    {
                        tocur.Text = currtitle;
                    }
                    //    papers.Items.AddRange (new string[] { newspapertitle, newspaperlink });
                    //  indexit = Convert.ToInt32(papers.SelectedItem);
                    countit++;

                }
                fromcur.DropDownStyle = ComboBoxStyle.DropDownList;
                tocur.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }


        private void python_Click(object sender, EventArgs e)
        {
            string appdata = Environment.ExpandEnvironmentVariables("%localappdata%");
            string website = appdata+"\\Programs\\Python\\Python36\\Lib\\idlelib\\idle.pyw";

            runproc(website, "Idle");
        }
        private void python_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Python Idle";
        }


        private void nav_to()
        {
            string textx;
            directory = dirlist.Text;
            textx = directory;
            dirlist.Text = dirlist.Text;
            try
            {
                rootname = "";
                string[] splitword = dirlist.Text.Split('\\');
                int counter = splitword.Count();
                if (counter == 1)
                {
                    rootname = splitword[0] + @"\";
                    dirlist.Text = rootname;
                }
                for (int i = 0; i < counter - 1; i++)
                {
                    if (i > 0)
                        rootname = rootname + @"\" + splitword[i];
                    else
                        rootname = splitword[i];
                }
                // textx = rootname;
                //     rootname = dirlist.Text;
                int count = dirlist.Items.Count;
                string tempdir = dirlist.Text;
           //     dirlist.Items.Clear();
                while (dirlist.Items.Count > 0)
                    dirlist.Items.RemoveAt(0);
                dirlist.Text = tempdir;
                DirectoryInfo directory = new DirectoryInfo(tempdir);
                try
                {
                    DirectoryInfo[] files = directory.GetDirectories();

                    var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));
                    dirlist.Items.Add(rootname);
                    dirlist.Items.Add(tempdir);
                    //  dirlist.Items.Clear();


                    foreach (var f in filtered)
                    {
                        dirlist.Items.Add((f.FullName));

                    }
                    dirlist.Text = tempdir;
                    //rootname = dirlist.Text;
                }
                catch
                {
                    dirlist.Text = "no directory";
                    dirlist.Items.Add(@"c:\");
                }
            }
            catch (UnauthorizedAccessException ex)
            {

            }
        }

        private void papers_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Newspapers";
            papers.DroppedDown = true;
        }

        private void papers_MouseLeave(object sender, EventArgs e)
        {
            excepterr.Text = "";
            papers.DroppedDown = false;
        }

        private void miscellaneous_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Miscellaneous Stuff";
            miscellaneous.DroppedDown = true;
        }

        private void miscellaneous_MouseLeave(object sender, EventArgs e)
        {
            excepterr.Text = "";
            miscellaneous.DroppedDown = false;
        }

        private void footie_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Football Links";
            footie.DroppedDown = true;
        }

        private void footie_MouseLeave(object sender, EventArgs e)
        {
            excepterr.Text = "";
            footie.DroppedDown = false;
        }

        private void tables_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Football Tables";
            tables.DroppedDown = true;
        }

        private void tables_MouseLeave(object sender, EventArgs e)
        {
            tables.DroppedDown = false;
            excepterr.Text = "";
        }

        void comboBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }


        private void drivelist_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int count = drivelist.Items.Count;
                rootname = drivelist.Text;
                dirlist.Items.Clear();
                DirectoryInfo directory = new DirectoryInfo(rootname);
                DirectoryInfo[] files = directory.GetDirectories();

                var filtered = files.Where(f => !f.Attributes.HasFlag(FileAttributes.Hidden));
                //  dirlist.Items.Add(@"\..");
                dirlist.Items.Add(rootname);

                foreach (var f in filtered)
                {
                    dirlist.Items.Add((f.FullName));
                }
                dirlist.Text = rootname;
            }
            catch (UnauthorizedAccessException ex)
            {

            }
        }

        private void totalmedia_Click(object sender, EventArgs e)
        {
            string path = "%programfiles(x86)%\\ArcSoft\\TotalMedia 3.5\\TotalMedia.exe";
            string website = Environment.ExpandEnvironmentVariables(path);

            runproc(website, "Total Media");

        }

        private void totalmedia_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Total Media";
        }


        private void netflix_Click(object sender, EventArgs e)
        {
            String testmatch = googlein.Text;

            testmatch = testregex(testmatch);

            string website = "\"https://www.netflix.com/search?q=" + testmatch + "\"";
            runproc(website, "Netflix");

        }
        private void netflix_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "Netflix";
        }


        private void winexp_Click(object sender, EventArgs e)
        {
            directory = @"c:\users\"+uname+@"\documents";
            System.Diagnostics.Process.Start(directory, "");

        }

        private void winexp_MouseHover(object sender, EventArgs e)
        {
            excepterr.Text = "document directory";
        }

        private void dirlist_MouseHover(object sender, EventArgs e)
        {
            nav_to();
            excepterr.Text = "Directory of "+ directory;
        }

        private void excepterr_MouseLeave(object sender, EventArgs e)
        {
            excepterr.Text = "             ";
        }

        private void drivelist_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void dirlist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}