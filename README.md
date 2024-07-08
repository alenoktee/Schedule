# **Attention!**
For the program to work correctly, you need XML files, which are located in the "bin.zip" archive, which must first be unpacked.
___

## **Task:**

Implement the program specified in the option using Windows Form technology. Be sure to use menus and toolbars. Provide for all exceptional situations. Create an application for creating a group class schedule. Provide the ability to create groups.


The program was developed in 3 days, taking into account switching to other disciplines in the college besides this one. For convenient data storage, **XML files**, **datatables** and **dataGridViews** were used. The goal was to make a simple project that would meet the requirements of the lab.

<img align="center" src="https://github.com/alenoktee/Schedule/blob/master/Main.png" width="55%"></img>
<img align="center" src="https://github.com/alenoktee/Schedule/blob/master/Edit.png" width="40%"></img>

### **Briefly about the application:**

On the main form you can select one of 5 days of the week and the department you are interested in. The table will display the schedule of all groups in this department for the selected day. Groups are sorted in ascending order (1st-4th year).

When creating a group, a new XML file is created with the name of this group, and the name of the group is placed in Groups.xml (contains a list of all groups).

An example of an XML file for one of the groups:
<img align="center" src="https://github.com/alenoktee/Schedule/blob/master/32i.png" width="55%"></img>

Then, when editing and saving the schedule, the program reads all lines from the datagridview and updates the file for this group.

Whenever groups or schedules change (within the application), the main form is updated.


---

<p align="center">I would be grateful if you write your opinion about the project! ☺️</p>
