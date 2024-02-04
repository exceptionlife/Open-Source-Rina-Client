using System;
using System.Collections.Generic;

namespace Rina.Client.Source_files
{
    class Texts
    {
        private static Dictionary<string, string> Text = new Dictionary<string, string>
        {
            {"UNKNOWNERROR",                    "Bilinmeyen, ancak kritik bir hata oldu .. Sorunu çözmeye yardımcı olabilecek hata mesajı:\n {0}"},
            {"MISSINGBINARY",                   "{0} eksik olduğundan oyun başlatılamıyor."},
            {"CANNOTSTART",                     "Oyuna başlayamazsınız, muhtemelen antivirüsünüzü lütfen kapatın."},
            {"NONETWORK",                       "Sunucuya bağlanılamıyor, lütfen ağ ayarlarınızı kontrol edin ve tekrar deneyin."},
            {"CONNECTING",                      "Sunucuya bağlanılıyor..."},
            {"LISTDOWNLOAD",                    "Güncelleme listesi indiriliyor..."},
            {"CHECKFILE",                       "{0} kontrol ediliyor..."},
            {"DOWNLOADFILE",                    "{0} indiriliyor... {1}/ {2}"},
            {"COMPLETEPROGRESS",                "Toplam İlerleme: {0}%"},
            {"CURRENTPROGRESS",                 "Dosya İlerlemesi: {0}%  |  {1} kb/s"},
            {"CHECKCOMPLETE",                   "Her dosya düzgün kontrol edildi."},
            {"DOWNLOADCOMPLETE",                "Gerekli tüm dosyalar düzgün şekilde indirildi."},
            {"DOWNLOADSPEED",                   "{0} kb/s"}
        };

        public static string GetText(string Key, params object[] Arguments)
        {
            foreach (var currentItem in Text)
            {
                if(currentItem.Key == Key)
                {
                    return string.Format(currentItem.Value, Arguments); 
                }
            }

            return null;
        }
    }
}
