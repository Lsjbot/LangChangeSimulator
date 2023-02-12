using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;

public class hbookclass
{
    private SortedDictionary<string, int> shist = new SortedDictionary<string, int>();
    private SortedDictionary<int, int> ihist = new SortedDictionary<int, int>();
    private int[,] d2hist = null;
    //private SortedDictionary<double, int> dhist = new SortedDictionary<double, int>();

    private const int MAXBINS = 202;
    private double[] binlimits = new double[MAXBINS];
    private double[] binlimits2 = new double[MAXBINS];
    int dimx = 0;
    int dimy = 0;
    private double binmax = 100;
    private double binmin = 0;
    private double binwid = 0;
    private int nbins = MAXBINS - 2;
    private double binmax2 = 100;
    private double binmin2 = 0;
    private double binwid2 = 0;
    private int nbins2 = MAXBINS - 2;
    private string name = "";
    private double sumx = 0;
    private double sumx2 = 0;

    public hbookclass(string namepar)
    {
        name = namepar;
    }

    public hbookclass(string namepar, int dimxpar, int dimypar)
    {
        name = namepar;
        dimx = dimxpar;
        dimy = dimypar;
        d2hist = new int[dimx+2, dimy+2];
        for (int i = 0; i <= dimx+1; i++)
            for (int j = 0; j <= dimy+1; j++)
                d2hist[i, j] = 0;

    }


    public void Add(string key)
    {
        if (!shist.ContainsKey(key))
            shist.Add(key, 1);
        else
            shist[key]++;
    }

    public void Add(char key)
    {

        if (!shist.ContainsKey(key.ToString()))
            shist.Add(key.ToString(), 1);
        else
            shist[key.ToString()]++;
    }

    public void Add(int key)
    {
        if (!ihist.ContainsKey(key))
            ihist.Add(key, 1);
        else
            ihist[key]++;
    }

    private int valuetobin(double key)
    {
        int bin = 0;
        if (key > binmin)
        {
            if (key > binmax)
                bin = nbins + 1;
            else
            {
                bin = (int)((key - binmin) / binwid) + 1;
            }
        }
        return bin;
    }

    private int valuetobin2(double key)
    {
        int bin = 0;
        if (key > binmin2)
        {
            if (key > binmax2)
                bin = nbins2 + 1;
            else
            {
                bin = (int)((key - binmin2) / binwid2) + 1;
            }
        }
        return bin;
    }

    private double bintomin(int bin)
    {
        if (bin == 0)
            return binmin;
        if (bin > nbins)
            return binmax;
        return binmin + (bin - 1) * binwid;
    }

    private double bintomax(int bin)
    {
        if (bin == 0)
            return binmin;
        if (bin > nbins)
            return binmax;
        return binmin + bin * binwid;
    }

    private double bintomin2(int bin)
    {
        if (bin == 0)
            return binmin2;
        if (bin > nbins2)
            return binmax2;
        return binmin2 + (bin - 1) * binwid2;
    }

    private double bintomax2(int bin)
    {
        if (bin == 0)
            return binmin2;
        if (bin > nbins2)
            return binmax2;
        return binmin2 + bin * binwid2;
    }

    public void Add(double key)
    {
        int bin = valuetobin(key);
        if (!ihist.ContainsKey(bin))
            ihist.Add(bin, 1);
        else
            ihist[bin]++;
        sumx += key;
        sumx2 += key * key;
    }

    public void Add(double key,double key2)
    {
        int bin = valuetobin(key);
        int bin2 = valuetobin(key2);
        d2hist[bin, bin2]++;
    }

    public void SetBins(double min, double max, int nb)
    {
        if (nbins > MAXBINS - 2)
        {
            Console.WriteLine("Too many bins. Max " + (MAXBINS - 2).ToString());
            return;
        }
        else
        {
            binmax = max;
            binmin = min;
            nbins = nb;
            binwid = (max - min) / nbins;
            binlimits[0] = binmin;
            for (int i = 1; i <= nbins; i++)
            {
                binlimits[i] = binmin + i * binwid;
            }

            if (dimy == 0)
            {
                for (int i = 0; i <= nbins + 1; i++)
                    if (!ihist.ContainsKey(i))
                        ihist.Add(i, 0);
            }
        }
    }

    public void SetBins2(double min, double max, int nb)
    {
        if (nbins > MAXBINS - 2)
        {
            Console.WriteLine("Too many bins. Max " + (MAXBINS - 2).ToString());
            return;
        }
        else
        {
            binmax2 = max;
            binmin2 = min;
            nbins2 = nb;
            binwid2 = (max - min) / nbins;
            binlimits2[0] = binmin;
            for (int i = 1; i <= nbins2; i++)
            {
                binlimits2[i] = binmin2 + i * binwid2;
            }

        }
    }

    public string getheader()
    {
        return name;
    }

    public void PrintIHist()
    {
        int total = 0;
        Console.WriteLine(getheader());
        //string s = "";
        foreach (int key in ihist.Keys)
        {
            Console.WriteLine(key + ": " + ihist[key].ToString());
            //s += key + ": " + ihist[key].ToString() + "\n";
            total += ihist[key];
        }
        Console.WriteLine("----Total : " + total.ToString());
        //s += "----Total : " + total.ToString() + "\n";
        //return s;
    }

    public string GetIHist()
    {
        int total = 0;
        double sum = 0;
        string s = getheader() + "\n";
        foreach (int key in ihist.Keys)
        {
            //Console.WriteLine(key + ": " + ihist[key].ToString());
            s += key + "\t" + ihist[key].ToString() + "\n";
            total += ihist[key];
            sum += key * ihist[key];
        }
        //Console.WriteLine("----Total : " + total.ToString());
        s += "----Total : " + total.ToString() + "\n";
        if (total > 0)
            s += "----Mean : " + (sum / total).ToString() + "\n";
        return s;
    }

    public void PrintDHist()
    {
        Console.WriteLine(getheader());
        int total = (from c in ihist select c.Value).Sum();
        foreach (int key in ihist.Keys)
        {
            Console.WriteLine(bintomin(key).ToString() + " -- " + bintomax(key).ToString() + "\t" + ihist[key].ToString()+"\t"+(double)ihist[key]/total);
            //total += ihist[key];
        }
        Console.WriteLine("----Total : " + total.ToString());
    }

    public string GetDHist()
    {
        StringBuilder sb = new StringBuilder(getheader()+"\n");
        int total = (from c in ihist select c.Value).Sum();
        foreach (int key in ihist.Keys)
        {
            sb.Append(bintomin(key).ToString() + " -- " + bintomax(key).ToString() + "\t" + ihist[key].ToString() + "\t" + (100*(double)ihist[key] / total).ToString("F2",new CultureInfo("sv-SE")) + "\n");
        }
        sb.Append("----Total : " + total.ToString()+"\n");
        sb.Append("--Average : " + sumx / total);
        return sb.ToString();
    }

    public string GetD2Hist()
    {
        StringBuilder sb = new StringBuilder(getheader()+"\n");
        int total = 0;
        for (int j = 0; j <= dimy; j++)
            sb.Append("\t" + bintomin2(j));
        sb.Append("\n");
        for (int i = 0; i <= dimx + 1; i++)
        {
            sb.Append(bintomin(i));
            for (int j = 0; j <= dimy + 1; j++)
                sb.Append("\t"+d2hist[i, j]);
        }

        sb.Append("\n----Total : " + total.ToString());
        return sb.ToString();
    }

    public void PrintSHist()
    {
        Console.WriteLine(getheader());
        int total = 0;
        foreach (string key in shist.Keys)
        {
            Console.WriteLine(key + ": " + shist[key].ToString());
            total += shist[key];
        }
        Console.WriteLine("----Total : " + total.ToString());
    }

    public string GetSHist()
    {
        int total = 0;
        string s = getheader()+"\n";
        foreach (string key in shist.Keys)
        {
            s += key + "\t" + shist[key].ToString()+"\n";
            total += shist[key];
        }
        s += "----Total : " + total.ToString() + "\n";
        return s;
    }
}
