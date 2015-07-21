using UnityEngine;
using System.Collections;
using System;

namespace Novel{

	public class FileManager
	{
        public FileManager(ILoader loader)
        {
            _loader = loader;
        }

        // Use this for initialization
		void Start ()
		{
			//	Debug.Log ("FileManger 初期化");
		}
		// Update is called once per frame
		void Update ()
		{
	
		}

        private ILoader _loader = null;

        // 外部ファイルからテキストを読み込む
        public string load(string filename)
        {
            return _loader.LoadText(filename);
        }

        public Sprite LoadSprite(string filename)
        {
            return _loader.LoadSprite(filename);
        }

        AudioClip LoadAudio(string filename)
        {
            return _loader.LoadAudio(filename);
        }
        //public string load(string file_name)
        //{

        //    // TextAssetとして、Resourcesフォルダからテキストデータをロードする
        //    TextAsset stageTextAsset = Resources.Load ("novel/data/" + file_name) as TextAsset;

        //    if (stageTextAsset == null) {
        //        NovelSingleton.GameManager.showError ("ファイル「" + file_name + "」が見つかりませんでした。");
        //    }

        //    // 文字列を代入
        //    string stageData = stageTextAsset.text;
        //    // 空白を置換で削除

        //    return stageData;

        //}
	}

    public interface ILoader
    {
        string LoadText(string filename);
        Sprite LoadSprite(string filename);
        AudioClip LoadAudio(string filename);
    }

    public class FileLoader : ILoader
    {
        public string LoadText(string filename)
        {
            string stageData = string.Empty;
            try
            {
                var target = System.IO.Directory.GetCurrentDirectory() + "/novel/data/" + filename;
                var file = FindFile(target);
                stageData = System.IO.File.ReadAllText(file);
            }
            catch (Exception/*ex*/)
            {
                NovelSingleton.GameManager.showError("ファイル「" + filename + "」が見つかりませんでした。");
            }

            return stageData;
        }

        public Sprite LoadSprite(string filename)
        {
            try
            {
                byte[] bindata = null;
                var target = System.IO.Directory.GetCurrentDirectory() + "/" + filename;
                var file = FindFile(target);
                bindata = System.IO.File.ReadAllBytes(file);
                System.Drawing.Image i = System.Drawing.Image.FromStream(new System.IO.MemoryStream(bindata));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                i.Save(ms, System.Drawing.Imaging.ImageFormat.Png); // 画像にGIFを使わなければこの変換は不要になるはず
                Sprite s = Sprite.Create(ReadTexture(ms.GetBuffer(), i.Width, i.Height), new Rect(0, 0, i.Width, i.Height), new Vector2(0.5f, 0.5f));
                return s;
            }
            catch (Exception/*ex*/)
            {
                NovelSingleton.GameManager.showError("ファイル「" + filename + "」が見つかりませんでした。");
            }
            return null;
        }

        public AudioClip LoadAudio(string filename)
        {
            // 音関連はアセットに取り込む形のほうがいいかも．ファイルから読むの大変そう
            try
            {
                byte[] bindata = null;
                var target = System.IO.Directory.GetCurrentDirectory() + "/" + filename;
                var file = FindFile(target);
                bindata = System.IO.File.ReadAllBytes(file);
                AudioSource s = new AudioSource();
                AudioClip c = AudioClip.Create(filename, bindata.Length, 2, 440, true);
                
                return c;
            }
            catch(Exception)
            {
                NovelSingleton.GameManager.showError("ファイル「" + filename + "」が見つかりませんでした。");
            }
            return null;
        }

        private string FindFile(string target)
        {
            System.IO.FileInfo fi = new System.IO.FileInfo(target);

            if (fi.Exists)
            {
                return fi.FullName;
            }
            else
            {
                var fil = fi.Directory.GetFiles(fi.Name + ".*");
                foreach (System.IO.FileInfo f in fil)
                {
                    if (f.Name.EndsWith("meta")) continue;
                    return fil[0].FullName;
                }
            }
            return string.Empty;
        }

        private Texture2D ReadTexture(byte[] readBinary, int width, int height)
        {
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.LoadImage(readBinary);
            texture.filterMode = FilterMode.Bilinear;

            return texture;
        }
    }
}