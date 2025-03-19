using System;
using System.IO;

namespace HSE_Bank.DataImportExport;

public abstract class DataImporter
{
    public void Import(string filePath)
    {
        string content = File.ReadAllText(filePath);
        var data = Preprocess(content);
        Parse(data);
        Postprocess();
    }

    protected virtual string Preprocess(string content) => content.Trim();

    protected abstract void Parse(string data);

    protected virtual void Postprocess()
    {
        Console.WriteLine("Импорт завершен!");
    }
}