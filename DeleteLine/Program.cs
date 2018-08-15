using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace DeleteLine
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // 指定されたファイルパス
                string filePath = "";
                if (args.Length > 0)
                {
                    filePath = args[0];
                }

                // 検索文字列
                List<string> findString = new List<string>();
                for (int i = 1; i < args.Length; i++)
                {
                    findString.Add(args[i]);
                }

                // 指定されたファイルパスからファイル一覧を検索
                string[] files = GetAllFiles(filePath);

                // ファイル毎に処理を行う
                foreach (string f in files)
                {
                    DeleteLine(f, findString);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static public void DeleteLine(string filePath, List<string> findString)
        {
            // ファイルオープン
            StreamReader reader = new StreamReader(filePath);

            // 1行ずつ読み込み
            List<string> writeString = new List<string>();
            string readString = "";
            while ((readString = reader.ReadLine()) != null)
            {
                // 検索文字列がない場合のみ書き込み
                bool bFind = false;
                foreach (string f in findString)
                {
                    if (readString.Contains(f))
                    {
                        bFind = true;
                        break;
                    }
                }

                if (!bFind)
                {
                    writeString.Add(readString);
                }
            }
            reader.Close();

            // ファイルの属性設定
            FileAttributes fa = 0;
            File.SetAttributes(filePath, fa);

            // ファイル書き込み
            StreamWriter writer = new StreamWriter(filePath,
                                                       false  // 上書き （ true = 追加 ）
                                                       );
            foreach (string f in writeString)
            {
                writer.WriteLine(f);
            }
            writer.Close();
        }


        // ファイル一覧の検索
        static public string[] GetAllFiles(string filePath)
        {
            try
            {
                // "*"を含む場合にファイル一覧を検索
                if (filePath.Contains("*"))
                {
                    // ディレクトリとファイル名に分ける
                    string directory = Path.GetDirectoryName(filePath);
                    string file = Path.GetFileName(filePath);

                    // ディレクトリが指定されなかった場合は、カレントディレクトリから検索
                    if (string.Empty == directory)
                    {
                        directory = Directory.GetCurrentDirectory();
                    }

                    // ディレクトリ以下のファイル名に一致するファイルを検索
                    return Directory.GetFiles(directory, file, SearchOption.AllDirectories);
                }
                else
                {
                    // 指定されたファイルのみ
                    return new string[1] { filePath };
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return new string[0];
            }
        }
    }
}
