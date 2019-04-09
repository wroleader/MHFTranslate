using System;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using Colorify;
using Colorify.UI;
using Google.Apis.Services;
using Google.Apis.Translate.v2;
using Google.Apis.Translate.v2.Data;
using TranslationsResource = Google.Apis.Translate.v2.Data.TranslationsResource;
using System.Linq;
using System.Collections.Generic;
using Google.Cloud.Translation.V2;
using System.Text;

namespace SSTranslator
{
    class Program
    {
        public static Format _colorify { get; set; }
        public static string ExeLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string TranslateTargetLanguage = "";

        public static string GApi = "";


 
        /*
        TranslateService service = new TranslateService(new BaseClientService.Initializer()
        {
            ApiKey = "", //Google API Key
            ApplicationName = "MHFTranslate" //AppName
        });
        */

        [STAThread]
        static void Main(string[] args)
        {
            _colorify = new Format(Theme.Dark);
            Console.Title = "Monster Hunter Frontier - Chat Translator";
            FolderBrowserDialog browsefolder = new FolderBrowserDialog();

            string[] allLangs = { "af", "sq", "ar", "az", "eu", "bn", "be", "bg", "ca", "zh-CN", "zh-TW", "hr", "cs", "da",
                                  "nl", "en", "eo", "et", "tl", "fi", "fr", "gl", "ka", "de", "el", "gu", "ht", "iw", "hi",
                                  "hu", "is", "id", "ga", "it", "kn", "ko", "la", "lv", "lt", "mk", "ms", "mt", "no", "fa",
                                  "pl", "pt", "ro", "ru", "sr", "sk", "sl", "es", "sw", "sv", "ta", "te", "th", "tr", "ur",
                                  "vi", "cy", "yi"};

            string logfolder = "";

            _colorify.AlignCenter("Starting up...", Colors.bgInfo);
            Console.WriteLine("\n You can terminate execution at any time by hitting CTRL+C\n");

            if(!File.Exists(ExeLocation + "\\translateConfig.txt"))
            {
                _colorify.WriteLine("Configuration file not found. Creating a new one...", Colors.bgWarning);
                using (FileStream fs = File.Create(ExeLocation + "\\translateConfig.txt")) { ; }

                _colorify.WriteLine("New configuration file created at: " + ExeLocation + "\\translateConfig.txt", Colors.bgSuccess);

                if (browsefolder.ShowDialog() == DialogResult.OK)
                {
                    logfolder = browsefolder.SelectedPath;
                    _colorify.WriteLine("\nSelected folder is: " + logfolder, Colors.bgSuccess);

                    Console.WriteLine("Type out the language code for what you'd like the chat to be translated to.\n" +
                                      "Type 'list' to get a list of languages. Setting will be saved in config file.\n" +
                                      "To change configuration settings, modify or delete translateConfig.txt in this program's folder.\n\n" +
                                      "Common languages: 'en' - English | 'es' - Spanish | 'pt' - Portuguese\n");

                    Console.Write("Please select your language: ");

                    TranslateTargetLanguage = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(TranslateTargetLanguage))
                    {
                        TranslateTargetLanguage = "en";
                        using (StreamWriter sw = File.CreateText(ExeLocation + "\\translateConfig.txt"))
                        {
                            sw.WriteLine(logfolder);
                            Console.WriteLine("Writing: " + logfolder);
                            sw.WriteLine(TranslateTargetLanguage);
                            Console.WriteLine("Writing: " + TranslateTargetLanguage);
                            sw.Flush();
                            sw.Close();
                        }
                        _colorify.AlignCenter("Configuration Complete", Colors.bgSuccess);
                        _colorify.AlignCenter("Language: " + TranslateTargetLanguage, Colors.bgSuccess);
                        _colorify.AlignCenter("Log folder: " + logfolder, Colors.bgSuccess);
                        System.Threading.Thread.Sleep(3000);
                        StartTranslation(logfolder, TranslateTargetLanguage);
                    }
                    else if (TranslateTargetLanguage == "list")
                    {
                        _colorify.AlignCenter("Afrikaans: af | Albanian: sq | Arabic: ar | Azerbaijani: az | Basque: eu", Colors.bgInfo);
                        _colorify.AlignCenter("Bengali: bn | Belarusian: be | Bulgarian: bg | Catalan: ca | Simpl. Chinese: zh-CN", Colors.bgInfo);
                        _colorify.AlignCenter("Tradit. Chinese: zh-TW | Croatian: hr | Czech: cs | Danish: da | Dutch: nl", Colors.bgInfo);
                        _colorify.AlignCenter("English: en | Esperanto: eo | Estonian: et | Filipino: tl | Finnish: fi | French: fr", Colors.bgInfo);
                        _colorify.AlignCenter("Galician: gl | Georgian: ka | German: de | Greek: el | Gujarati: gu | Haitian Creole: ht", Colors.bgInfo);
                        _colorify.AlignCenter("Hebrew: iw | Hindi: hi | Hungarian: hu | Icelandic: is | Indonesian: id | Irish: ga", Colors.bgInfo);
                        _colorify.AlignCenter("Italian: it | Kannada: kn | Korean: ko | Latin: la | Latvian: lv | Lithuanian: lt", Colors.bgInfo);
                        _colorify.AlignCenter("Macedonian: mk | Malay: ms | Maltese: mt | Norwegian: no | Persian: fa | Polish: pl", Colors.bgInfo);
                        _colorify.AlignCenter("Portuguese: pt | Romanian: ro | Russian: ru | Serbian: sr | Slovak: sk | Slovenian: sl", Colors.bgInfo);
                        _colorify.AlignCenter("Spanish: es | Swahili: sw | Swedish: sv | Tamil: ta | Telugu: te | Thai: th | Turkish: tr", Colors.bgInfo);
                        _colorify.AlignCenter("Ukrainian: uk | Urdu: ur | Vietnamese: vi | Welsh: cy | Yiddish: yi", Colors.bgInfo);
                        Console.WriteLine("");
                        Console.WriteLine("");
                        _colorify.Write("Please select your language: ");

                        TranslateTargetLanguage = Console.ReadLine();

                        using (StreamWriter sw = File.CreateText(ExeLocation + "\\translateConfig.txt"))
                        {
                            sw.WriteLine(logfolder);
                            sw.WriteLine(TranslateTargetLanguage);
                            sw.Flush();
                            sw.Close();
                        }

                        _colorify.AlignCenter("Configuration Complete", Colors.bgSuccess);
                        _colorify.AlignCenter("Language: " + TranslateTargetLanguage, Colors.bgSuccess);
                        _colorify.AlignCenter("Log folder: " + logfolder, Colors.bgSuccess);
                        System.Threading.Thread.Sleep(3000);
                        StartTranslation(logfolder, TranslateTargetLanguage);
                    }
                    else
                    {
                        using (StreamWriter sw = File.CreateText(ExeLocation + "\\translateConfig.txt"))
                        {
                            sw.WriteLine(logfolder);
                            Console.WriteLine("Writing: " + logfolder);
                            sw.WriteLine(TranslateTargetLanguage);
                            Console.WriteLine("Writing: " + TranslateTargetLanguage);
                            sw.Flush();
                            sw.Close();
                        }
                        _colorify.AlignCenter("Configuration Complete", Colors.bgSuccess);
                        _colorify.AlignCenter("Language: " + TranslateTargetLanguage, Colors.bgSuccess);
                        _colorify.AlignCenter("Log folder: " + logfolder, Colors.bgSuccess);
                        System.Threading.Thread.Sleep(3000);
                        StartTranslation(logfolder, TranslateTargetLanguage);
                    }
                }
                else
                {
                    _colorify.Clear();
                    _colorify.WriteLine("Folder selection failed.\n ERR-CODE: SELECTION_CANCELLED_OR_ABORTED", Colors.txtWarning);
                    ShowQuestFailure();
                    _colorify.WriteLine("Configurations not saved.", Colors.bgWarning);
                    if (File.Exists(ExeLocation + "\\translateConfig.txt"))
                    {
                        File.Delete(ExeLocation + "\\translateConfig.txt");
                    }
                }
            }
            else
            {
                _colorify.WriteLine("");
                _colorify.WriteLine("Reading configuration file: ");
                string[] lines = File.ReadAllLines(ExeLocation + "\\translateConfig.txt");
                logfolder = lines[0];
                _colorify.WriteLine("\nLog folder at: " + logfolder);
                TranslateTargetLanguage = lines[1];
                _colorify.WriteLine("Configured language: " + TranslateTargetLanguage);
                System.Threading.Thread.Sleep(3000);
                _colorify.Clear();
                StartTranslation(logfolder, TranslateTargetLanguage);
            }
            Console.ReadKey();
            _colorify.ResetColor();
            _colorify.Clear();
        }

        static void ShowQuestFailure()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(".......................,:....:..................................................\n....................,::...,:..:,................................................\n..................:... ..ZZZ~. ,,...............................................\n................:....?Z7.,ZZZ=..,,...........................:....:,............\n..............,....7ZZ,$Z..ZZZ~..,,.......................,:....~...:...........\n.............:. .ZZZZZZZZZ~.ZZZ$..,,....................,,...,ZZZI....:.........\n............:.....Z$?ZZZ,ZZ?.ZZZ?...,.................,:....$ZZZZ~.:...:........\n...........,..+ZZ:.?ZZZZZ,7$..$ZZ?...,..............:....~ZZZZZ~.7ZZZ...:.......\n...........,..~ZZZ?.~,:ZZ$..:=.+ZZ?...,...........:,...=ZZZZZ,.+ZZ$.ZZ.. :......\n............: ..ZZZZ.:ZZ:.~ZZZ..:ZZ$...:........,,...7ZZZZZ:.$ZZ?~ZZZZZ?..:,....\n.............:...+ZZZ,...$ZZ7~ZZ.,ZZZ...:.....,:...~ZZZZ$..=.+?=ZZZZ=~$,~..:....\n..............,:..,ZZZ+.=Z,ZZZZZ..,ZZZ...:...:...~ZZZZ$..7,ZZ,~ZZ?=ZZ.?ZZ..,....\n................:..........,,+?~ZZ+.ZZZ,..,,....ZZZZ$...ZZZIZZ~.$Z~.IZZZZ.......\n..............,:....:ZI....... ...........:~..ZZZZI..ZZ$.ZZZZ.ZZ..ZZZZ$,...:....\n.............,....7Z=.7ZI....,,....:........ .........ZZZZ$,ZZ~.7ZZZZ~...,,.....\n.............:..$ZZ,...?ZZ,..Z,....ZZ$:ZZ7,~,==7,..............~II$+...,,.......\n.............:..ZZ...I.,ZZ..?Z....,ZZ:=ZZ, .+ZZZ,...IZZZZ.$ZI~,......,,.........\n.............:..+ZZ:.,ZZZ: .ZZ....:ZZ.+Z7$?,...,.$$:.....,==?$$$$$..:...........\n.............,....=Z:Z+ZZ,.?Z$....,Z,.~Z,$ZZZ$,....$Z=.......7......,...........\n.............,,:,...,..,Z~.?ZZZZZZZ7..7$............,ZZZ+....Z...,,.............\n...........,.............. ..,........=.?ZZZZZZ=....:?ZZZ...+Z...,..............\n...........:..$I............................,?Z,+IZZZZZZZ...ZZ:..,..............\n...........:..ZZZ7ZZZZZ7...+.... ..........................:ZZ~..,..............\n...........:..ZZ7...~ZZ+..ZZ....ZZZZ..=7Z.......................,...............\n...........~..ZZ.ZZZ$,...ZZZ7...ZZZI.7ZZ~.......ZZ7.~++II,.......:,.............\n..........,:..Z:,ZI,....?...,...ZZZ..?ZZ........ZZ...=ZZZ,$ZZ.Z=...:............\n..........,,.:+...... +ZZ..~Z=..ZZ:..IZ~........Z$7$:. .,.=ZZ.~Z$.. ,,..........\n........,:.. ,.......7ZZZ+IZZZ..ZZ.. +$..... ...Z.$ZZ$7+..:Z=..ZZZ=...,.........\n......,:....??~:...........,:I+.7,...I.,+7ZZZZ..7......,. ,Z,..ZZZZ7..,.........\n.....:....$ZZZZ~.$Z?+ZZZZ:...............:?$$Z:,,=ZZZZZZ=..Z..ZZZ=...:..........\n....:...~ZZZZ=..Z=~ZZZ~~ZZ..,ZZZZ. ::................,$Z~..Z.~Z?.. .:...........\n...:..=ZZZZ~.ZZ,:ZZZZZZ~,.:ZZZZ=...:.:..7ZZ,,Z$7,:,..............,:.............\n...:..ZZ$:?Z7,ZZ=.ZZ:=,.:ZZZZ7...,,..,...7ZZ~..=ZZZZ?$$.,ZZZ~...~...............\n...,..:,=+~ZZZZZ:7.I:.+ZZZZZ,...:.....,,..7ZZ~.?Z$:ZZZ:...ZZZ7...:..............\n....:...ZZZZZ$~ZZZ..=ZZZZZ:...:,.......,,..~ZZ=..IZZ7..IZ~.IZZZ,..:,............\n.....:...ZZ=+ZZZ,.7ZZZZZ... :,..........,,..=ZZ$.,$..~ZZ$.~.~ZZZ?..,............\n......:...ZZZZ~.IZZZZ7.. .:,.............,,..~ZZZ...Z~ZZZZZZ.:ZZZ..:............\n.......,...~:.7ZZZZI....:,................,:..~ZZZ..ZZ?IZZ7,$..~. .:............\n........:....7ZZZ?....:,....................:..:ZZZ.,Z$$ZZZZZZ....,.............\n..........,:..I+....:........................,..:ZZZ:.ZZ~?ZZI...:...............\n............: ...,,...........................:..,ZZZ~.~ZZ:...,:................\n..............,,...............................:..:ZZZ:.....,,..................\n................................................: ..7....,:,....................\n.................................................: ....:,.......................\n....................................................,...........................\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key or CTRL+C to terminate the program.");
        }

        static void StartTranslation(string mhflogs, string targetlang)
        { 
            //This is where we begin translations.
            //We prepare a translation client, which we will create from our Google API Key.
            TranslationClient gTranslate = TranslationClient.CreateFromApiKey(GApi);

            _colorify.Clear();
            _colorify.AlignCenter("Reading Logs", Colors.bgInfo);
            _colorify.DivisionLine('-');

            //First, we get the most recently modified file in the game's logs folder.
            var mhLogFolder = new DirectoryInfo(mhflogs);
            var lastLog = mhLogFolder.GetFiles()
                .OrderByDescending(f => f.LastWriteTime).FirstOrDefault();

            string pathToLog = mhLogFolder + "\\" + lastLog;

            //pathToLog includes both the folder & .txt
           
            foreach (string line in TailFrom(pathToLog))
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    TranslationResult result = gTranslate.TranslateText(line, targetlang, LanguageCodes.Japanese);
                    if (line.Contains("SYSTEM"))
                    {
                        _colorify.WriteLine(result.TranslatedText, Colors.txtWarning);
                    }
                    else if (line.Contains("FIELD"))
                    {
                        _colorify.WriteLine(result.TranslatedText, Colors.txtInfo);
                    }
                    else
                    {
                        _colorify.WriteLine(result.TranslatedText, Colors.txtSuccess);
                    }
                }
            }
        }
        static IEnumerable<string> TailFrom(string file)
        {
            using (var reader = new StreamReader(file, Encoding.Default))
            {
                while (true)
                {
                    string line = reader.ReadLine();
                    if (reader.BaseStream.Length < reader.BaseStream.Position)
                        reader.BaseStream.Seek(0, SeekOrigin.Begin);

                    if (line != null) yield return line;
                    else System.Threading.Thread.Sleep(500);
                }
            }
        }
    }
}
