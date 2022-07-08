using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Abraham
{
    public class Serializer<T> where T:new()
    {
        #region ------------- Eigenschaften -------------------------------------------------------
        #endregion



        #region ------------- Felder --------------------------------------------------------------

        private string Dateiname;
        private string DateinameBak1;
        private string DateinameBak2;

        #endregion



        #region ------------- Init ----------------------------------------------------------------

        public Serializer(string dateiname)
        {
            Dateiname     = dateiname;
            DateinameBak1 = Dateiname + ".bak";
            DateinameBak2 = Dateiname + ".bak2";
        }

        #endregion



        #region ------------- Methoden ------------------------------------------------------------

        public T Load()
        {
            try
            {
                return TryLoad(Dateiname);
            }
            catch (Exception) { }

            try
            {
                return TryLoad(DateinameBak1);
            }
            catch (Exception) { }

            try
            {
                return TryLoad(DateinameBak2);
            }
            catch (Exception) { }

            return new T();
        }

        public T TryLoad(string dateiname)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            FileStream fs = new FileStream(dateiname, FileMode.Open);
            T Daten = (T)serializer.Deserialize(fs);
            fs.Close();
            return Daten;
        }

        public T Deserialize(string input)
        {
            string TempDir = Environment.GetEnvironmentVariable("TEMP");
            string TempFileName = TempDir + Path.DirectorySeparatorChar + "24309572309485729598327454365";
            
            File.WriteAllText(TempFileName, input);
            var Daten = TryLoad(@"\\server1\hausnet$\Data\Datenobjekte.xml.bak2");
            File.Delete(TempFileName);
            
            return Daten;
        }

        public void Save(T daten)
        {
            if (daten == null)
                return;

            if (File.Exists(DateinameBak1))
                File.Copy(DateinameBak1, DateinameBak2, overwrite: true);
            
            if (File.Exists(Dateiname))
                File.Copy(Dateiname, DateinameBak1, overwrite:true);

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                FileStream fs = new FileStream(Dateiname, FileMode.Create);
                serializer.Serialize(fs, daten);
                fs.Close();
            }
            catch (Exception ex)
            {
                if (File.Exists(Dateiname))
                    File.Copy(DateinameBak1, Dateiname, overwrite: true);
                throw;
            }
        }

        #endregion



        #region ------------- Implementation ------------------------------------------------------
        #endregion
    }
}
