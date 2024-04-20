# **Attention!**
For the program to work correctly, you need XML files, which are located in the "bin.zip" archive, which must first be unpacked.
___
## Exercise:

Implement the program specified in the option using Windows Form technology. Be sure to use menus and toolbars. Provide for all exceptional situations. Create an application for creating a group class schedule. Provide the ability to create groups.

The program was developed in 3 days, taking into account switching to other disciplines in the college besides this one. XML files were used to store data. The goal was to make a simple project that would meet the requirements of the lab.

![Скриншот](https://github.com/alenoktee/Schedule/blob/master/Main.png "Главная форма")
![Скриншот](https://github.com/alenoktee/Schedule/blob/master/Edit.png "Изменение расписания")


'''c#
    
private DataTable time()
{
    XDocument loadedData = XDocument.Load("time.xml");

    DataTable dt = new DataTable();
    dt.Columns.Add("Время");

    foreach (XElement xelement in loadedData.Element("root").Elements())
    {
        DataRow row = dt.NewRow();
        row["Время"] = xelement.Value;
        dt.Rows.Add(row);
    }

    return dt;
}

'''
