using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;
using System.Data;
using System.ComponentModel;
using System.Globalization;
using System.Xml;
using System.Text.RegularExpressions;
using System.Runtime.Serialization.Json;
using System.Linq.Expressions;
using Newtonsoft.Json;

public static class MethodExtensions
{
  

    /// <summary>
    /// Transform object into Identity data type (integer).
    /// </summary>
    /// <param name="item">The object to be transformed.</param>
    /// <param name="defaultId">Optional default value is -1.</param>
    /// <returns>Identity value.</returns>
    public static int? AsId(this object item, int? defaultId = null)
    {
        if (item == null)
            return defaultId;

        int result;
        if (!int.TryParse(item.ToString(), out result))
            return defaultId;

        return result;
    }


    /// <summary>
    /// Transform object into integer data type.
    /// </summary>
    /// <param name="item">The object to be transformed.</param>
    /// <param name="defaultId">Optional default value is default(int).</param>
    /// <returns>The integer value.</returns>
    public static int AsInt(this object item, int defaultInt = default(int))
    {
        if (item == null)
            return defaultInt;

        int result;
        if (!int.TryParse(item.ToString(), out result))
            return defaultInt;

        return result;
    }


    /// <summary>
    /// Transform object into double data type.
    /// </summary>
    /// <param name="item">The object to be transformed.</param>
    /// <param name="defaultId">Optional default value is default(double).</param>
    /// <returns>The double value.</returns>
    public static double AsDouble(this object item, double defaultDouble = default(double))
    {
        if (item == null)
            return defaultDouble;

        double result;
        if (!double.TryParse(item.ToString(), out result))
            return defaultDouble;

        return result;
    }

    /// <summary>
    /// Transform object into decimal data type.
    /// </summary>
    /// <param name="item">The object to be transformed.</param>
    /// <param name="defaultId">Optional default value is default(double).</param>
    /// <returns>The double value.</returns>
    public static decimal AsDecimal(this object item, decimal defaultDecimal = default(decimal))
    {
        if (item == null)
            return defaultDecimal;

        decimal result;
        if (!decimal.TryParse(item.ToString(), out result))
            return defaultDecimal;

        return result;
    }

    /// <summary>
    /// Converte texto para decimal mesmo tendo mascara
    /// EX: 10.00 %, R$ 12
    /// </summary>
    /// <param name="item"></param>
    /// <param name="defaultDecimal"></param>
    /// <returns></returns>
    public static decimal AsDecimalMask(this object item, decimal defaultDecimal = default(decimal))
    {
        if (item == null)
            return defaultDecimal;
        var valor = "";

        if (item.ToString().Contains("%"))
        {

            valor = Regex.Replace(item.ToString(), "[^0-9-.]", "");
            valor = valor.Replace(".", ",");
        }
        else
        {
            valor = Regex.Replace(item.ToString(), "[^0-9-,]", "");
            //valor = valor.Replace(",", ".");
        }
        decimal result;

        if (!decimal.TryParse(valor, out result))
            return defaultDecimal;

        return result;
    }

    public static decimal AsDecimalUSMask(this object item, decimal defaultDecimal = default(decimal))
    {
        if (item == null)
            return defaultDecimal;
        var valor = "";

        valor = Regex.Replace(item.ToString(), "[^0-9-.]", "");
        valor = valor.Replace(".", ",");

        decimal result;

        if (!decimal.TryParse(valor, out result))
            return defaultDecimal;

        return result;
    }

    public static string DecimalToStringRS(this decimal dec)
    {
        dec = dec.AsDecimal();

        return dec.ToString("C2");
    }

    /// <summary>
    /// Transform object into string data type.
    /// </summary>
    /// <param name="item">The object to be transformed.</param>
    /// <param name="defaultId">Optional default value is default(string).</param>
    /// <returns>The string value.</returns>
    public static string AsString(this object item, string defaultString = default(string))
    {
        if (item == null || item.Equals(System.DBNull.Value))
            return defaultString;

        return item.ToString().Trim();
    }

    /// <summary>
    /// Transform object into DateTime data type.
    /// </summary>
    /// <param name="item">The object to be transformed.</param>
    /// <param name="defaultId">Optional default value is default(DateTime).</param>
    /// <returns>The DateTime value.</returns>
    public static DateTime AsDateTime(this object item, DateTime defaultDateTime = default(DateTime))
    {
        if (item == null || string.IsNullOrEmpty(item.ToString()))
            return defaultDateTime;

        DateTime result;
        if (!DateTime.TryParse(item.ToString(), out result) || result == DateTime.MinValue)
            return defaultDateTime;

        return result;
    }

    /// <summary>
    /// Transform object into DateTime data type.
    /// </summary>
    /// <param name="item">The object to be transformed.</param>
    /// <param name="defaultId">Optional default value is default(DateTime).</param>
    /// <returns>The DateTime value.</returns>
    public static DateTime? AsDateTimeNull(this object item, DateTime? defaultDateTime = null)
    {
        if (item == null || string.IsNullOrEmpty(item.ToString()))
            return defaultDateTime;

        DateTime result;
        if (!DateTime.TryParse(item.ToString(), out result))
            return defaultDateTime;

        return result;
    }

    public static DateTime? AsDateTimeBR(this object item, DateTime? defaultDateTime = null)
    {
        DateTime? result = null;
        if (item == null || string.IsNullOrEmpty(item.ToString()))
            return defaultDateTime;

        try
        {
            result = DateTime.ParseExact(item.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (Exception ex)
        {
        }

        return result;
    }

    public static DateTime FirstDayOfMonth(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, 1);
    }

    public static int DaysInMonth(this DateTime value)
    {
        return DateTime.DaysInMonth(value.Year, value.Month);
    }

    public static DateTime LastDayOfMonth(this DateTime value)
    {
        return new DateTime(value.Year, value.Month, value.DaysInMonth());
    }

   

    /// <summary>
    /// Formata a data em dd/MM/yyyy
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string FormateData(DateTime? item)
    {
        string result = "";
        if (item != null || item.HasValue)
        {
            result = item.Value.ToString("dd/MM/yyyy");
        }

        return result;
    }
    /// <summary>
    /// Transform object into bool data type.
    /// </summary>
    /// <param name="item">The object to be transformed.</param>
    /// <param name="defaultId">Optional default value is default(bool).</param>
    /// <returns>The bool value.</returns>
    public static bool AsBool(this object item, bool defaultBool = default(bool))
    {
        if (item == null)
            return defaultBool;

        return new List<string>() { "yes", "y", "true", "sim" }.Contains(item.ToString().ToLower());
    }

    /// <summary>
    /// Transform string into byte array.
    /// </summary>
    /// <param name="s">The object to be transformed.</param>
    /// <returns>The transformed byte array.</returns>
    public static byte[] AsByteArray(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return null;

        return Convert.FromBase64String(s);
    }

    /// <summary>
    /// Transform object into base64 string.
    /// </summary>
    /// <param name="item">The object to be transformed.</param>
    /// <returns>The base64 string value.</returns>
    public static string AsBase64String(this object item)
    {
        if (item == null)
            return null;
        ;

        return Convert.ToBase64String((byte[])item);
    }

    /// <summary>
    /// Transform object into Guid data type.
    /// </summary>
    /// <param name="item">The object to be transformed.</param>
    /// <returns>The Guid value.</returns>
    public static Guid AsGuid(this object item)
    {
        try { return new Guid(item.ToString()); }
        catch { return Guid.Empty; }
    }

    public static long AsLong(this object item, long defaultDouble = default(long))
    {
        if (item == null)
            return defaultDouble;

        long result;
        if (!long.TryParse(item.ToString(), out result))
            return defaultDouble;

        return result;
    }

    public static byte? AsByte(this string s)
    {
        if (string.IsNullOrEmpty(s))
            return null;

        return Convert.ToByte(s);
    }

    public static string AsBooltoString(bool s)
    {
        return s ? "Sim" : "Não";
    }

    public static string AsShortDate(DateTime? item)
    {
        string ret = "";

        if (item.HasValue)
            ret = item.AsDateTime().ToString("dd/MM/yyyy");

        return ret;
    }

    /// <summary>
    /// Concatenates SQL and ORDER BY clauses into a single string. 
    /// </summary>
    /// <param name="sql">The SQL string</param>
    /// <param name="sortExpression">The Sort Expression.</param>
    /// <returns>Contatenated SQL Statement.</returns>
    public static string OrderBy(this string sql, string sortExpression)
    {
        if (string.IsNullOrEmpty(sortExpression))
            return sql;

        return sql + " ORDER BY " + sortExpression;
    }

    public static string SomenteNumeros(this object item)
    {
        var texto = "";
        if (item != null)
            texto = item.ToString();

        return Regex.Replace(texto, "[^0-9]", "");
    }

    public static string RemoveEspacoDuplicado(this string item)
    {
        var texto = "";
        if (item != null)
            texto = item.ToString();

        return string.Join(" ", texto.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
    }
  
    /// <summary>
    /// Remove os acentos da string.
    /// </summary>
    /// <param name="texto">string com texto</param>
    /// <returns>nova string sem acentos</returns>
    public static string RemoveAcentos(this string texto)
    {
        StringBuilder sb = new StringBuilder();
        if (!string.IsNullOrEmpty(texto))
        {
            texto = texto.Normalize(NormalizationForm.FormD);

            foreach (char c in texto.ToCharArray())
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
        }
        return sb.ToString();
    }

    public static string FormatCPF(this string cpf)
    {
        string MascaraCPF;

        if (cpf != null && cpf != "")
            MascaraCPF = string.Format("{0:00000000000}", Convert.ToDouble(cpf)).Insert(9, "-").Insert(6, ".").Insert(3, ".");
        else
            MascaraCPF = "";

        return MascaraCPF;
    }

    public static string FormatCNPJ(this string cnpj)
    {
        string MascaraCNPJ;

        if (cnpj != null && cnpj != "")
            MascaraCNPJ = string.Format("{0:00000000000000}", Convert.ToDouble(cnpj)).Insert(12, "-").Insert(8, "/").Insert(5, ".").Insert(2, ".");
        else
            MascaraCNPJ = "";

        return MascaraCNPJ;
    }

    public static string FormatCEP(this string cep)
    {
        return string.Format("{0:00000000}", Convert.ToDouble(cep)).Insert(5, "-");
    }

    public static string FormatTelefoneFixo(this string telefoneFixo)
    {
        string TelefoneF;

        if (telefoneFixo != null && telefoneFixo != "")
            TelefoneF = string.Format("{0:0000000000}", Convert.ToDouble(telefoneFixo)).Insert(0, "(").Insert(3, ") ").Insert(8, "-");
        else
            TelefoneF = "";

        return TelefoneF;
    }

    public static string FormatTelefoneCelular(this string telefoneCelular)
    {
        string TelefoneC;

        if (telefoneCelular != null && telefoneCelular != "")
            TelefoneC = string.Format("{0:00000000000}", Convert.ToDouble(telefoneCelular)).Insert(0, "(").Insert(3, ") ").Insert(9, "-");
        else
            TelefoneC = "";

        return TelefoneC;
    }

    /// <summary>
    /// Devolve um pedaço do texto começando a contar pela esquerda
    /// </summary>
    /// <param name="that"></param>
    /// <param name="tamanho">Quantidade de caracteres a serem retornados</param>
    /// <returns>Texto</returns>
    public static string Left(this string that, int tamanho)
    {
        return that.Substring(0, Math.Min(tamanho, that.Length));
    }

    /// <summary>
    /// Devolve um pedaço do texto começando a contar pela direita
    /// </summary>
    /// <param name="that"></param>
    /// <param name="tamanho"></param>
    /// <returns>texto</returns>
    public static string Right(this string that, int tamanho)
    {
        int comecaEm = that.Length - tamanho;

        if (comecaEm < 0)
            comecaEm = 0;

        return that.Substring(comecaEm, Math.Min(tamanho, that.Length));
    }

    public static string FormatarTelefone(this string telefone)
    {
        var idxtel = 5;
        if (telefone.Length == 10)
            idxtel = 4;

        var ddd = telefone.Left(2);
        var tel1 = telefone.Substring(2, idxtel);
        var tel2 = telefone.Substring(idxtel + 2);

        return $"({ddd}) {tel1}-{tel2}";
    }

    public static T CastClass<T>(this Object myobj, string dataFormatada)
    {
        Type objectType = myobj.GetType();
        Type target = typeof(T);
        var x = Activator.CreateInstance(target, false);
        var z = from source in objectType.GetMembers().ToList()
                where source.MemberType == MemberTypes.Property
                select source;
        var d = from source in target.GetMembers().ToList()
                where source.MemberType == MemberTypes.Property
                select source;
        List<MemberInfo> members = d.Where(memberInfo => d.Select(c => c.Name)
           .ToList().Contains(memberInfo.Name)).ToList();
        PropertyInfo propertyInfo;
        object value;
        foreach (var memberInfo in members)
        {
            propertyInfo = typeof(T).GetProperty(memberInfo.Name);
            value = myobj.GetType().GetProperty(memberInfo.Name).GetValue(myobj, null);

            propertyInfo.SetValue(x, value, null);
        }
        return (T)x;
    }

    public static string PastaExiste(this string CaminhoArquivo)
    {
        if (!Directory.Exists(CaminhoArquivo))
        {
            Directory.CreateDirectory(CaminhoArquivo);
        }

        return CaminhoArquivo;
    }

    public static string Trim(object item)
    {
        string ret = "";

        if (item != null)
            ret = RemoveAcentos(item.ToString().Trim().Replace(';', ',').Replace("\r\n", " "));

        return ret;
    }

  

    /// <summary>
    /// REMOVE CARACTERES ESPECIAIS (^~´Ç $ & etc..)
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string RemoverCharsEspeciais(string text)
    {
        StringBuilder sbReturn = new StringBuilder();
        if (!string.IsNullOrEmpty(text))
        {
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
        }
        return Regex.Replace(sbReturn.ToString(), "[^ A-Za-z0-9]", "");
    }

    public static string RemoverLetras(string text)
    {
        return Regex.Replace(text, "[^0-9]", "");
    }

    public static string DecimalToStringUs(this decimal dec)
    {
        return dec.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
    }

    public static string ToBase64String(this string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }

    public static string AsDateTimeNullString(this object item, DateTime? defaultDateTime = null)
    {
        if (item == null || string.IsNullOrEmpty(item.ToString()))
            return "";

        DateTime result;
        if (!DateTime.TryParse(item.ToString(), out result))
            return "";

        return result.ToShortDateString();
    }

    /// <summary>
    /// Ira tirar o percentula do valor2 / valor
    /// </summary>
    /// <param name="valor"></param>
    /// <param name="valor2"></param>
    /// <returns>percentual do valor2 sobre o valor</returns>
    public static decimal Percentual(decimal valor, decimal valor2)
    {
        decimal percentual = 0;

        if (valor == 0)
            return 0;

        if (valor2 == 0)
            return 0;
        percentual = (valor2 / valor) * 100;

        return percentual;
    }

   

    public static String GetMimeFromFileName(String fileName)
    {
        return GetFromExtension(Path.GetExtension(fileName).Remove(0, 1));
    }

    public static String GetFromExtension(String ext)
    {
        if (MIMETypesDictionary.ContainsKey(ext))
        {
            return MIMETypesDictionary[ext];
        }
        return "unknown/unknown";
    }

    private static IDictionary<string, string> MIMETypesDictionary = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase) {
            {"323", "text/h323"},
            {"3g2", "video/3gpp2"},
            {"3gp", "video/3gpp"},
            {"3gp2", "video/3gpp2"},
            {"3gpp", "video/3gpp"},
            {"7z", "application/x-7z-compressed"},
            {"aa", "audio/audible"},
            {"AAC", "audio/aac"},
            {"aaf", "application/octet-stream"},
            {"aax", "audio/vnd.audible.aax"},
            {"ac3", "audio/ac3"},
            {"aca", "application/octet-stream"},
            {"accda", "application/msaccess.addin"},
            {"accdb", "application/msaccess"},
            {"accdc", "application/msaccess.cab"},
            {"accde", "application/msaccess"},
            {"accdr", "application/msaccess.runtime"},
            {"accdt", "application/msaccess"},
            {"accdw", "application/msaccess.webapplication"},
            {"accft", "application/msaccess.ftemplate"},
            {"acx", "application/internet-property-stream"},
            {"AddIn", "text/xml"},
            {"ade", "application/msaccess"},
            {"adobebridge", "application/x-bridge-url"},
            {"adp", "application/msaccess"},
            {"ADT", "audio/vnd.dlna.adts"},
            {"ADTS", "audio/aac"},
            {"afm", "application/octet-stream"},
            {"ai", "application/postscript"},
            {"aif", "audio/x-aiff"},
            {"aifc", "audio/aiff"},
            {"aiff", "audio/aiff"},
            {"air", "application/vnd.adobe.air-application-installer-package+zip"},
            {"amc", "application/x-mpeg"},
            {"application", "application/x-ms-application"},
            {"art", "image/x-jg"},
            {"asa", "application/xml"},
            {"asax", "application/xml"},
            {"ascx", "application/xml"},
            {"asd", "application/octet-stream"},
            {"asf", "video/x-ms-asf"},
            {"ashx", "application/xml"},
            {"asi", "application/octet-stream"},
            {"asm", "text/plain"},
            {"asmx", "application/xml"},
            {"aspx", "application/xml"},
            {"asr", "video/x-ms-asf"},
            {"asx", "video/x-ms-asf"},
            {"atom", "application/atom+xml"},
            {"au", "audio/basic"},
            {"avi", "video/x-msvideo"},
            {"axs", "application/olescript"},
            {"bas", "text/plain"},
            {"bcpio", "application/x-bcpio"},
            {"bin", "application/octet-stream"},
            {"bmp", "image/bmp"},
            {"c", "text/plain"},
            {"cab", "application/octet-stream"},
            {"caf", "audio/x-caf"},
            {"calx", "application/vnd.ms-office.calx"},
            {"cat", "application/vnd.ms-pki.seccat"},
            {"cc", "text/plain"},
            {"cd", "text/plain"},
            {"cdda", "audio/aiff"},
            {"cdf", "application/x-cdf"},
            {"cer", "application/x-x509-ca-cert"},
            {"chm", "application/octet-stream"},
            {"class", "application/x-java-applet"},
            {"clp", "application/x-msclip"},
            {"cmx", "image/x-cmx"},
            {"cnf", "text/plain"},
            {"cod", "image/cis-cod"},
            {"config", "application/xml"},
            {"contact", "text/x-ms-contact"},
            {"coverage", "application/xml"},
            {"cpio", "application/x-cpio"},
            {"cpp", "text/plain"},
            {"crd", "application/x-mscardfile"},
            {"crl", "application/pkix-crl"},
            {"crt", "application/x-x509-ca-cert"},
            {"cs", "text/plain"},
            {"csdproj", "text/plain"},
            {"csh", "application/x-csh"},
            {"csproj", "text/plain"},
            {"css", "text/css"},
            {"csv", "text/csv"},
            {"cur", "application/octet-stream"},
            {"cxx", "text/plain"},
            {"dat", "application/octet-stream"},
            {"datasource", "application/xml"},
            {"dbproj", "text/plain"},
            {"dcr", "application/x-director"},
            {"def", "text/plain"},
            {"deploy", "application/octet-stream"},
            {"der", "application/x-x509-ca-cert"},
            {"dgml", "application/xml"},
            {"dib", "image/bmp"},
            {"dif", "video/x-dv"},
            {"dir", "application/x-director"},
            {"disco", "text/xml"},
            {"dll", "application/x-msdownload"},
            {"dll.config", "text/xml"},
            {"dlm", "text/dlm"},
            {"doc", "application/msword"},
            {"docm", "application/vnd.ms-word.document.macroEnabled.12"},
            {"docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {"dot", "application/msword"},
            {"dotm", "application/vnd.ms-word.template.macroEnabled.12"},
            {"dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            {"dsp", "application/octet-stream"},
            {"dsw", "text/plain"},
            {"dtd", "text/xml"},
            {"dtsConfig", "text/xml"},
            {"dv", "video/x-dv"},
            {"dvi", "application/x-dvi"},
            {"dwf", "drawing/x-dwf"},
            {"dwp", "application/octet-stream"},
            {"dxr", "application/x-director"},
            {"eml", "message/rfc822"},
            {"emz", "application/octet-stream"},
            {"eot", "application/octet-stream"},
            {"eps", "application/postscript"},
            {"etl", "application/etl"},
            {"etx", "text/x-setext"},
            {"evy", "application/envoy"},
            {"exe", "application/octet-stream"},
            {"exe.config", "text/xml"},
            {"fdf", "application/vnd.fdf"},
            {"fif", "application/fractals"},
            {"filters", "Application/xml"},
            {"fla", "application/octet-stream"},
            {"flr", "x-world/x-vrml"},
            {"flv", "video/x-flv"},
            {"fsscript", "application/fsharp-script"},
            {"fsx", "application/fsharp-script"},
            {"generictest", "application/xml"},
            {"gif", "image/gif"},
            {"group", "text/x-ms-group"},
            {"gsm", "audio/x-gsm"},
            {"gtar", "application/x-gtar"},
            {"gz", "application/x-gzip"},
            {"h", "text/plain"},
            {"hdf", "application/x-hdf"},
            {"hdml", "text/x-hdml"},
            {"hhc", "application/x-oleobject"},
            {"hhk", "application/octet-stream"},
            {"hhp", "application/octet-stream"},
            {"hlp", "application/winhlp"},
            {"hpp", "text/plain"},
            {"hqx", "application/mac-binhex40"},
            {"hta", "application/hta"},
            {"htc", "text/x-component"},
            {"htm", "text/html"},
            {"html", "text/html"},
            {"htt", "text/webviewhtml"},
            {"hxa", "application/xml"},
            {"hxc", "application/xml"},
            {"hxd", "application/octet-stream"},
            {"hxe", "application/xml"},
            {"hxf", "application/xml"},
            {"hxh", "application/octet-stream"},
            {"hxi", "application/octet-stream"},
            {"hxk", "application/xml"},
            {"hxq", "application/octet-stream"},
            {"hxr", "application/octet-stream"},
            {"hxs", "application/octet-stream"},
            {"hxt", "text/html"},
            {"hxv", "application/xml"},
            {"hxw", "application/octet-stream"},
            {"hxx", "text/plain"},
            {"i", "text/plain"},
            {"ico", "image/x-icon"},
            {"ics", "application/octet-stream"},
            {"idl", "text/plain"},
            {"ief", "image/ief"},
            {"iii", "application/x-iphone"},
            {"inc", "text/plain"},
            {"inf", "application/octet-stream"},
            {"inl", "text/plain"},
            {"ins", "application/x-internet-signup"},
            {"ipa", "application/x-itunes-ipa"},
            {"ipg", "application/x-itunes-ipg"},
            {"ipproj", "text/plain"},
            {"ipsw", "application/x-itunes-ipsw"},
            {"iqy", "text/x-ms-iqy"},
            {"isp", "application/x-internet-signup"},
            {"ite", "application/x-itunes-ite"},
            {"itlp", "application/x-itunes-itlp"},
            {"itms", "application/x-itunes-itms"},
            {"itpc", "application/x-itunes-itpc"},
            {"IVF", "video/x-ivf"},
            {"jar", "application/java-archive"},
            {"java", "application/octet-stream"},
            {"jck", "application/liquidmotion"},
            {"jcz", "application/liquidmotion"},
            {"jfif", "image/pjpeg"},
            {"jnlp", "application/x-java-jnlp-file"},
            {"jpb", "application/octet-stream"},
            {"jpe", "image/jpeg"},
            {"jpeg", "image/jpeg"},
            {"jpg", "image/jpeg"},
            {"js", "application/x-javascript"},
            {"json", "application/json"},
            {"jsx", "text/jscript"},
            {"jsxbin", "text/plain"},
            {"latex", "application/x-latex"},
            {"library-ms", "application/windows-library+xml"},
            {"lit", "application/x-ms-reader"},
            {"loadtest", "application/xml"},
            {"lpk", "application/octet-stream"},
            {"lsf", "video/x-la-asf"},
            {"lst", "text/plain"},
            {"lsx", "video/x-la-asf"},
            {"lzh", "application/octet-stream"},
            {"m13", "application/x-msmediaview"},
            {"m14", "application/x-msmediaview"},
            {"m1v", "video/mpeg"},
            {"m2t", "video/vnd.dlna.mpeg-tts"},
            {"m2ts", "video/vnd.dlna.mpeg-tts"},
            {"m2v", "video/mpeg"},
            {"m3u", "audio/x-mpegurl"},
            {"m3u8", "audio/x-mpegurl"},
            {"m4a", "audio/m4a"},
            {"m4b", "audio/m4b"},
            {"m4p", "audio/m4p"},
            {"m4r", "audio/x-m4r"},
            {"m4v", "video/x-m4v"},
            {"mac", "image/x-macpaint"},
            {"mak", "text/plain"},
            {"man", "application/x-troff-man"},
            {"manifest", "application/x-ms-manifest"},
            {"map", "text/plain"},
            {"master", "application/xml"},
            {"mda", "application/msaccess"},
            {"mdb", "application/x-msaccess"},
            {"mde", "application/msaccess"},
            {"mdp", "application/octet-stream"},
            {"me", "application/x-troff-me"},
            {"mfp", "application/x-shockwave-flash"},
            {"mht", "message/rfc822"},
            {"mhtml", "message/rfc822"},
            {"mid", "audio/mid"},
            {"midi", "audio/mid"},
            {"mix", "application/octet-stream"},
            {"mk", "text/plain"},
            {"mmf", "application/x-smaf"},
            {"mno", "text/xml"},
            {"mny", "application/x-msmoney"},
            {"mod", "video/mpeg"},
            {"mov", "video/quicktime"},
            {"movie", "video/x-sgi-movie"},
            {"mp2", "video/mpeg"},
            {"mp2v", "video/mpeg"},
            {"mp3", "audio/mpeg"},
            {"mp4", "video/mp4"},
            {"mp4v", "video/mp4"},
            {"mpa", "video/mpeg"},
            {"mpe", "video/mpeg"},
            {"mpeg", "video/mpeg"},
            {"mpf", "application/vnd.ms-mediapackage"},
            {"mpg", "video/mpeg"},
            {"mpp", "application/vnd.ms-project"},
            {"mpv2", "video/mpeg"},
            {"mqv", "video/quicktime"},
            {"ms", "application/x-troff-ms"},
            {"msi", "application/octet-stream"},
            {"mso", "application/octet-stream"},
            {"mts", "video/vnd.dlna.mpeg-tts"},
            {"mtx", "application/xml"},
            {"mvb", "application/x-msmediaview"},
            {"mvc", "application/x-miva-compiled"},
            {"mxp", "application/x-mmxp"},
            {"nc", "application/x-netcdf"},
            {"nsc", "video/x-ms-asf"},
            {"nws", "message/rfc822"},
            {"ocx", "application/octet-stream"},
            {"oda", "application/oda"},
            {"odc", "text/x-ms-odc"},
            {"odh", "text/plain"},
            {"odl", "text/plain"},
            {"odp", "application/vnd.oasis.opendocument.presentation"},
            {"ods", "application/oleobject"},
            {"odt", "application/vnd.oasis.opendocument.text"},
            {"one", "application/onenote"},
            {"onea", "application/onenote"},
            {"onepkg", "application/onenote"},
            {"onetmp", "application/onenote"},
            {"onetoc", "application/onenote"},
            {"onetoc2", "application/onenote"},
            {"orderedtest", "application/xml"},
            {"osdx", "application/opensearchdescription+xml"},
            {"p10", "application/pkcs10"},
            {"p12", "application/x-pkcs12"},
            {"p7b", "application/x-pkcs7-certificates"},
            {"p7c", "application/pkcs7-mime"},
            {"p7m", "application/pkcs7-mime"},
            {"p7r", "application/x-pkcs7-certreqresp"},
            {"p7s", "application/pkcs7-signature"},
            {"pbm", "image/x-portable-bitmap"},
            {"pcast", "application/x-podcast"},
            {"pct", "image/pict"},
            {"pcx", "application/octet-stream"},
            {"pcz", "application/octet-stream"},
            {"pdf", "application/pdf"},
            {"pfb", "application/octet-stream"},
            {"pfm", "application/octet-stream"},
            {"pfx", "application/x-pkcs12"},
            {"pgm", "image/x-portable-graymap"},
            {"pic", "image/pict"},
            {"pict", "image/pict"},
            {"pkgdef", "text/plain"},
            {"pkgundef", "text/plain"},
            {"pko", "application/vnd.ms-pki.pko"},
            {"pls", "audio/scpls"},
            {"pma", "application/x-perfmon"},
            {"pmc", "application/x-perfmon"},
            {"pml", "application/x-perfmon"},
            {"pmr", "application/x-perfmon"},
            {"pmw", "application/x-perfmon"},
            {"png", "image/png"},
            {"pnm", "image/x-portable-anymap"},
            {"pnt", "image/x-macpaint"},
            {"pntg", "image/x-macpaint"},
            {"pnz", "image/png"},
            {"pot", "application/vnd.ms-powerpoint"},
            {"potm", "application/vnd.ms-powerpoint.template.macroEnabled.12"},
            {"potx", "application/vnd.openxmlformats-officedocument.presentationml.template"},
            {"ppa", "application/vnd.ms-powerpoint"},
            {"ppam", "application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            {"ppm", "image/x-portable-pixmap"},
            {"pps", "application/vnd.ms-powerpoint"},
            {"ppsm", "application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            {"ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            {"ppt", "application/vnd.ms-powerpoint"},
            {"pptm", "application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            {"pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            {"prf", "application/pics-rules"},
            {"prm", "application/octet-stream"},
            {"prx", "application/octet-stream"},
            {"ps", "application/postscript"},
            {"psc1", "application/PowerShell"},
            {"psd", "application/octet-stream"},
            {"psess", "application/xml"},
            {"psm", "application/octet-stream"},
            {"psp", "application/octet-stream"},
            {"pub", "application/x-mspublisher"},
            {"pwz", "application/vnd.ms-powerpoint"},
            {"qht", "text/x-html-insertion"},
            {"qhtm", "text/x-html-insertion"},
            {"qt", "video/quicktime"},
            {"qti", "image/x-quicktime"},
            {"qtif", "image/x-quicktime"},
            {"qtl", "application/x-quicktimeplayer"},
            {"qxd", "application/octet-stream"},
            {"ra", "audio/x-pn-realaudio"},
            {"ram", "audio/x-pn-realaudio"},
            {"rar", "application/octet-stream"},
            {"ras", "image/x-cmu-raster"},
            {"rat", "application/rat-file"},
            {"rc", "text/plain"},
            {"rc2", "text/plain"},
            {"rct", "text/plain"},
            {"rdlc", "application/xml"},
            {"resx", "application/xml"},
            {"rf", "image/vnd.rn-realflash"},
            {"rgb", "image/x-rgb"},
            {"rgs", "text/plain"},
            {"rm", "application/vnd.rn-realmedia"},
            {"rmi", "audio/mid"},
            {"rmp", "application/vnd.rn-rn_music_package"},
            {"roff", "application/x-troff"},
            {"rpm", "audio/x-pn-realaudio-plugin"},
            {"rqy", "text/x-ms-rqy"},
            {"rtf", "application/rtf"},
            {"rtx", "text/richtext"},
            {"ruleset", "application/xml"},
            {"s", "text/plain"},
            {"safariextz", "application/x-safari-safariextz"},
            {"scd", "application/x-msschedule"},
            {"sct", "text/scriptlet"},
            {"sd2", "audio/x-sd2"},
            {"sdp", "application/sdp"},
            {"sea", "application/octet-stream"},
            {"searchConnector-ms", "application/windows-search-connector+xml"},
            {"setpay", "application/set-payment-initiation"},
            {"setreg", "application/set-registration-initiation"},
            {"settings", "application/xml"},
            {"sgimb", "application/x-sgimb"},
            {"sgml", "text/sgml"},
            {"sh", "application/x-sh"},
            {"shar", "application/x-shar"},
            {"shtml", "text/html"},
            {"sit", "application/x-stuffit"},
            {"sitemap", "application/xml"},
            {"skin", "application/xml"},
            {"sldm", "application/vnd.ms-powerpoint.slide.macroEnabled.12"},
            {"sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide"},
            {"slk", "application/vnd.ms-excel"},
            {"sln", "text/plain"},
            {"slupkg-ms", "application/x-ms-license"},
            {"smd", "audio/x-smd"},
            {"smi", "application/octet-stream"},
            {"smx", "audio/x-smd"},
            {"smz", "audio/x-smd"},
            {"snd", "audio/basic"},
            {"snippet", "application/xml"},
            {"snp", "application/octet-stream"},
            {"sol", "text/plain"},
            {"sor", "text/plain"},
            {"spc", "application/x-pkcs7-certificates"},
            {"spl", "application/futuresplash"},
            {"src", "application/x-wais-source"},
            {"srf", "text/plain"},
            {"SSISDeploymentManifest", "text/xml"},
            {"ssm", "application/streamingmedia"},
            {"sst", "application/vnd.ms-pki.certstore"},
            {"stl", "application/vnd.ms-pki.stl"},
            {"sv4cpio", "application/x-sv4cpio"},
            {"sv4crc", "application/x-sv4crc"},
            {"svc", "application/xml"},
            {"swf", "application/x-shockwave-flash"},
            {"t", "application/x-troff"},
            {"tar", "application/x-tar"},
            {"tcl", "application/x-tcl"},
            {"testrunconfig", "application/xml"},
            {"testsettings", "application/xml"},
            {"tex", "application/x-tex"},
            {"texi", "application/x-texinfo"},
            {"texinfo", "application/x-texinfo"},
            {"tgz", "application/x-compressed"},
            {"thmx", "application/vnd.ms-officetheme"},
            {"thn", "application/octet-stream"},
            {"tif", "image/tiff"},
            {"tiff", "image/tiff"},
            {"tlh", "text/plain"},
            {"tli", "text/plain"},
            {"toc", "application/octet-stream"},
            {"tr", "application/x-troff"},
            {"trm", "application/x-msterminal"},
            {"trx", "application/xml"},
            {"ts", "video/vnd.dlna.mpeg-tts"},
            {"tsv", "text/tab-separated-values"},
            {"ttf", "application/octet-stream"},
            {"tts", "video/vnd.dlna.mpeg-tts"},
            {"txt", "text/plain"},
            {"u32", "application/octet-stream"},
            {"uls", "text/iuls"},
            {"user", "text/plain"},
            {"ustar", "application/x-ustar"},
            {"vb", "text/plain"},
            {"vbdproj", "text/plain"},
            {"vbk", "video/mpeg"},
            {"vbproj", "text/plain"},
            {"vbs", "text/vbscript"},
            {"vcf", "text/x-vcard"},
            {"vcproj", "Application/xml"},
            {"vcs", "text/plain"},
            {"vcxproj", "Application/xml"},
            {"vddproj", "text/plain"},
            {"vdp", "text/plain"},
            {"vdproj", "text/plain"},
            {"vdx", "application/vnd.ms-visio.viewer"},
            {"vml", "text/xml"},
            {"vscontent", "application/xml"},
            {"vsct", "text/xml"},
            {"vsd", "application/vnd.visio"},
            {"vsi", "application/ms-vsi"},
            {"vsix", "application/vsix"},
            {"vsixlangpack", "text/xml"},
            {"vsixmanifest", "text/xml"},
            {"vsmdi", "application/xml"},
            {"vspscc", "text/plain"},
            {"vss", "application/vnd.visio"},
            {"vsscc", "text/plain"},
            {"vssettings", "text/xml"},
            {"vssscc", "text/plain"},
            {"vst", "application/vnd.visio"},
            {"vstemplate", "text/xml"},
            {"vsto", "application/x-ms-vsto"},
            {"vsw", "application/vnd.visio"},
            {"vsx", "application/vnd.visio"},
            {"vtx", "application/vnd.visio"},
            {"wav", "audio/wav"},
            {"wave", "audio/wav"},
            {"wax", "audio/x-ms-wax"},
            {"wbk", "application/msword"},
            {"wbmp", "image/vnd.wap.wbmp"},
            {"wcm", "application/vnd.ms-works"},
            {"wdb", "application/vnd.ms-works"},
            {"wdp", "image/vnd.ms-photo"},
            {"webarchive", "application/x-safari-webarchive"},
            {"webtest", "application/xml"},
            {"wiq", "application/xml"},
            {"wiz", "application/msword"},
            {"wks", "application/vnd.ms-works"},
            {"WLMP", "application/wlmoviemaker"},
            {"wlpginstall", "application/x-wlpg-detect"},
            {"wlpginstall3", "application/x-wlpg3-detect"},
            {"wm", "video/x-ms-wm"},
            {"wma", "audio/x-ms-wma"},
            {"wmd", "application/x-ms-wmd"},
            {"wmf", "application/x-msmetafile"},
            {"wml", "text/vnd.wap.wml"},
            {"wmlc", "application/vnd.wap.wmlc"},
            {"wmls", "text/vnd.wap.wmlscript"},
            {"wmlsc", "application/vnd.wap.wmlscriptc"},
            {"wmp", "video/x-ms-wmp"},
            {"wmv", "video/x-ms-wmv"},
            {"wmx", "video/x-ms-wmx"},
            {"wmz", "application/x-ms-wmz"},
            {"wpl", "application/vnd.ms-wpl"},
            {"wps", "application/vnd.ms-works"},
            {"wri", "application/x-mswrite"},
            {"wrl", "x-world/x-vrml"},
            {"wrz", "x-world/x-vrml"},
            {"wsc", "text/scriptlet"},
            {"wsdl", "text/xml"},
            {"wvx", "video/x-ms-wvx"},
            {"x", "application/directx"},
            {"xaf", "x-world/x-vrml"},
            {"xaml", "application/xaml+xml"},
            {"xap", "application/x-silverlight-app"},
            {"xbap", "application/x-ms-xbap"},
            {"xbm", "image/x-xbitmap"},
            {"xdr", "text/plain"},
            {"xht", "application/xhtml+xml"},
            {"xhtml", "application/xhtml+xml"},
            {"xla", "application/vnd.ms-excel"},
            {"xlam", "application/vnd.ms-excel.addin.macroEnabled.12"},
            {"xlc", "application/vnd.ms-excel"},
            {"xld", "application/vnd.ms-excel"},
            {"xlk", "application/vnd.ms-excel"},
            {"xll", "application/vnd.ms-excel"},
            {"xlm", "application/vnd.ms-excel"},
            {"xls", "application/vnd.ms-excel"},
            {"xlsb", "application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            {"xlsm", "application/vnd.ms-excel.sheet.macroEnabled.12"},
            {"xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            {"xlt", "application/vnd.ms-excel"},
            {"xltm", "application/vnd.ms-excel.template.macroEnabled.12"},
            {"xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            {"xlw", "application/vnd.ms-excel"},
            {"xml", "text/xml"},
            {"xmta", "application/xml"},
            {"xof", "x-world/x-vrml"},
            {"XOML", "text/plain"},
            {"xpm", "image/x-xpixmap"},
            {"xps", "application/vnd.ms-xpsdocument"},
            {"xrm-ms", "text/xml"},
            {"xsc", "application/xml"},
            {"xsd", "text/xml"},
            {"xsf", "text/xml"},
            {"xsl", "text/xml"},
            {"xslt", "text/xml"},
            {"xsn", "application/octet-stream"},
            {"xss", "application/xml"},
            {"xtp", "application/octet-stream"},
            {"xwd", "image/x-xwindowdump"},
            {"z", "application/x-compress"},
            {"zip", "application/x-zip-compressed"}
      };

    public static int geraPontosSenha(string senha)
    {
        if (senha == null) return 0;
        int pontosPorTamanho = GetPontoPorTamanho(senha);
        int pontosPorMinusculas = GetPontoPorMinusculas(senha);
        int pontosPorMaiusculas = GetPontoPorMaiusculas(senha);
        int pontosPorDigitos = GetPontoPorDigitos(senha);
        int pontosPorSimbolos = GetPontoPorSimbolos(senha);
        int pontosPorRepeticao = GetPontoPorRepeticao(senha);
        return pontosPorTamanho + pontosPorMinusculas + pontosPorMaiusculas + pontosPorDigitos + pontosPorSimbolos - pontosPorRepeticao;
    }

    private static int GetPontoPorTamanho(string senha)
    {
        return Math.Min(10, senha.Length) * 6;
    }

    private static int GetPontoPorMinusculas(string senha)
    {
        int rawplacar = senha.Length - Regex.Replace(senha, "[a-z]", "").Length;
        return Math.Min(2, rawplacar) * 5;
    }

    private static int GetPontoPorMaiusculas(string senha)
    {
        int rawplacar = senha.Length - Regex.Replace(senha, "[A-Z]", "").Length;
        return Math.Min(2, rawplacar) * 5;
    }

    private static int GetPontoPorDigitos(string senha)
    {
        int rawplacar = senha.Length - Regex.Replace(senha, "[0-9]", "").Length;
        return Math.Min(2, rawplacar) * 5;
    }

    private static int GetPontoPorSimbolos(string senha)
    {
        int rawplacar = Regex.Replace(senha, "[a-zA-Z0-9]", "").Length;
        return Math.Min(2, rawplacar) * 5;
    }

    private static int GetPontoPorRepeticao(string senha)
    {
        System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"(\w)*.*\1");
        bool repete = regex.IsMatch(senha);
        if (repete)
        {
            return 30;
        }
        else
        {
            return 0;
        }
    }

    public static ForcaDaSenha GetForcaDaSenha(string senha)
    {
        int placar = geraPontosSenha(senha);

        if (placar < 50)
            return ForcaDaSenha.Inaceitavel;
        else if (placar < 60)
            return ForcaDaSenha.Fraca;
        else if (placar < 80)
            return ForcaDaSenha.Aceitavel;
        else if (placar < 100)
            return ForcaDaSenha.Forte;
        else
            return ForcaDaSenha.Segura;
    }

    public enum ForcaDaSenha
    {
        Inaceitavel,
        Fraca,
        Aceitavel,
        Forte,
        Segura
    }

}
