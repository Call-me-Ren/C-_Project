Module 3 -- Writing C# code for Windows forms.

In previous week, we learned the basics of creating graphical user
interfaces, which are basically windows forms composed of some controls.
These forms will not have any functionally without code linked to the
forms and their controls. For example, if we want to perform some
operations when user clicks on a button, we need to write the proper
code inside the click event handler of the button.

In this chapter, we will write our first C# code. For this, please
create a new empty C# windows forms project in Visual Studio. Name your
project as *Module3_1.*

# 1) Switching between Code and Design views 

Your project will have an initial form named **Form1** which you can
view in the Solution Explorer window (you can open it from the *View*
menu if this window is not visible). We will open the source code file
for Form1. To do that, inside the Solution Explorer window, please right
click on the Form1.cs item, which will open a pop-up menu. In this menu,
click on **View Code** (See Figure 1).

> ![](./assets/module-3-writing-c-code/media/image0.jpg)

**Figure 1.** Solution Explorer window.

Alternatively, you can press *F7* to view the code for the selected form
(and switch back to the design view by pressing on *Shift+F7*).

The source code file should be opened as shown in Figure 2. You may
notice that the Toolbox and Properties windows have no content in the
*Code* view. This is because these windows serve for no purpose in
coding, and they are mainly intended for designing GUI.

![](./assets/module-3-writing-c-code/media/image2.jpg){width="6.0513899825021875in"
height="3.25625in"}

**Figure 2.** Visual Studio environment when code view is enabled.

# 2) Understanding the structure of the source code file for the form 

We will look into the details of the source code file. The C# code is
mainly composed of four components: using statements, namespace, class,
and methods. These components are marked in Figure 3.

> ![](./assets/module-3-writing-c-code/media/image210.jpg)

**Figure 3.** Components of the source code file.

(1) **using statements** always go to the top of the page and they help
    import *.Net namespaces* which may contain useful classes that we
    can take advantage to perform some operations easily and quickly.
    For examples, importing System.Text namespace would allow you to use
    the StringBuilder class, which provides very useful methods to work
    with strings. By default, popular namespaces are included in the
    code file for your convenience.

(2) **namespace** is the overarching container for the classes.
    Namespace block is automatically created in the code with the name
    of your project (which is *Week3_1*). Namespaces always have an
    opening and closing **{** **}** braces (see Figure 4). Any code that
    goes between these braces belongs to the namespace.

> ![](./assets/module-3-writing-c-code/media/image38.jpg)

**Figure 4.** Opening and closing braces for the namespace.

(3) **class** serves as the container for the methods that you will
    write for performing some operations based on user interaction with
    the interface (e.g., pressing enter, clicking button). C# is an
    object-oriented programming language, and everything is built based
    on the classes.

While you can create your custom classes (which will be covered in the
following weeks), **frm_main** class (derived from **Form** class) is
automatically generated to allow us to write the necessary code to add
some functionality to our form. Like namespaces, classes always have an
opening and closing braces **{** **}** (see Figure 5). Any code that
goes between these braces belongs to that class. For now, do discard the
**public** and **partial** keywords. These concepts will be covered in
the following weeks.

> ![](./assets/module-3-writing-c-code/media/image38.jpg)

**Figure 5.** Opening and closing braces for the class.

(4) a default **method** named **frm_main** is created in our Form1
    class. This method contains a systemdefined method called
    InitializeComponent(). You should leave this method as it is and
    should never change or remove it.

> ![](./assets/module-3-writing-c-code/media/image38.jpg)

**Figure 6.** Opening and closing braces for the method.

# 3) Writing the first C# code to implement a page load event 

It is time to get our hands dirty with coding. What we want to achieve
is very simple: to display a different text (other than "Initial
Message") in the label when the form is loaded for the first time. We
will write a single line of code but learn many things. Before starting
to code, we will have several preparations in the design view.

If you are in the code view, please press Shift + F7 to switch back to
the design view. Click on any part of the form to selected it. Then,
using the Properties window, set a **Text** (which will be the title of
the form) and a **Name** for the form as indicated in Figure 7.

> ![](./assets/module-3-writing-c-code/media/image40.jpg)

Next, add a label control around the centre of the form as seen in
Figure 8. Set some initial text (Initial Message) for the label and give
it a proper name (lbl_message).

> ![](./assets/module-3-writing-c-code/media/image50.jpg)

**Figure 8.** Adding a label control and setting a name and text value.

Now we are ready to start coding. Our goal is to change the text of the
label when the page is loaded. C# provides *form loading* method that is
triggered when the form is created and loaded in runtime for the first
time. Any code written inside this method will be executed when the form
is loaded after running the application. To access this method, we will
double click on any part of the form (where there is no control). Double
clicking on a control (such as label in our case) will generate a
different method. So, be careful!

Double click on the form. This will generate the form load method inside
the source code file. An automatic name was generated for this method
following this structure: {*form name*}**\_Load**. Since the form name
in our case is *frm_main*, the form load method was set to
**frm_main_Load** as seen in Figure 9.

> ![](./assets/module-3-writing-c-code/media/image60.jpg)

**Figure 9.** Source code file with Form Load method generated.

Whatever code we write here will be executed when the form is created
and loaded for the first time in runtime. What we want to do is to
change the text of the label (lbl_message) during form load. To
reference a control in the code view, you need to type its name (in the
correct line in the source code). Before writing the code, click inside
the **frm_main_Load** method to place the curser inside this method.

Now you are ready to write your first code. Start typing the name of the
label, which is lbl_message. When you type only the letter *l*, a popup
window will appear to provide some smart suggestions about what you
might be searching for (see Figure 10). The first item in the list
(which is already selected as indicated with blue background) is what we
are looking for.

> ![](./assets/module-3-writing-c-code/media/image8.jpg){width="3.4852777777777777in"
> height="2.0409722222222224in"}

**Figure 10.** Smart suggestions for coding.

Please press *Enter* or *Tab* key to quickly add lbl_message as seen in
Figure 11 below.

> ![](./assets/module-3-writing-c-code/media/image9.jpg){width="3.901388888888889in"
> height="0.7707217847769029in"}

**Figure 11.** lbl_message is added to the code.

These smart suggestions are part of the **IntelliSense** technology
available in Visual Studio. IntelliSense anticipates what you want to
type and helps you speed up the coding process. IntelliSense has many
more features and you will notice them as you continue practice writing
code. For example, if you add a period (.) just after lbl_message, a new
window will popup which includes a list of items associated with the
label control. See Figure 12 for the illustration.

![](./assets/module-3-writing-c-code/media/image10.jpg){width="5.847916666666666in"
height="1.2707917760279965in"}

**Figure 12.** Intelligence popup list after typing (.)

We choose the Text item in the list and continue writing our code. We
need to change the label text, in other words, we want to *assign* a new
string to the **Text** property of the label. To assign a new value, we
use the assignment operator ( = ). Since the Text property accepts only
strings, we provide a string value inside quotation marks: "Form is
loaded! :)", as seen in Figure 13.

![](./assets/module-3-writing-c-code/media/image11.jpg){width="4.860416666666667in"
height="2.687222222222222in"}

**Figure 13.** The code for assigning a string to a label text.

Now, we are ready to run and test our application. To do that we have
two options under the **Debug** menu: Start Debugging \[F5\] or Start
Without Debugging \[Ctrl+F5\] (see Figure 14).

  -------------------

  -------------------

![](./assets/module-3-writing-c-code/media/image12.jpg){width="5.553472222222222in"
height="0.7847222222222222in"}

**Figure 14.** Debugging options in the Debug menu.

Start Debugging \[F5\] option is also available in the toolbar as shown
in Figure 15.

> ![](./assets/module-3-writing-c-code/media/image120.jpg)

If you run your application with debugging, you can add breakpoints to
track what is happening in your code line by line. For now, we do not
need the debugging option since we have a simple code. Please press
Ctrl+F5 in your keyboard to run your application without debugging. You
should receive a warning about the errors in your code as you can see
Figure 16. We have some errors in our code, and Visual Studio warn us
about them before running our application.

![](./assets/module-3-writing-c-code/media/image14.jpg){width="5.752361111111111in"
height="2.3833333333333333in"}

**Figure 16.** Debugging option in the toolbar.

Please click NO, which should open the **Error List** window on the
bottom of the code window as seen in Figure 17. The error message says
"*; expected*", which is actually highlighted in the code for your
convenience with a **jagged red line**.

> ![](./assets/module-3-writing-c-code/media/image140.jpg)

Visual Studio analyses your code instantly as you type to identify the
syntax errors (like grammar errors in a spoken language). As an example,
please change *.Text* with .text in your code to intentionally throw an
error. There should appear a jagged line just under the .text as you can
see in Figure 18 below. While these errors are listed in the Error List
window, you can also mouse over on each jagged line to get more
information about each error.

![](./assets/module-3-writing-c-code/media/image17.jpg){width="5.360416666666667in"
height="2.456111111111111in"}

**Figure 18.** Error messages in the application.

Please fix your code by changing .text back to .Text. Let's focus on the
first error we received. Mouse over the jagged line to see the error
message.

> ![](./assets/module-3-writing-c-code/media/image170.jpg)

The error tells us that we need to add a **semicolon (** ; **)**
somewhere in the code. To get more information about the error, you can
click on the Error code (CS1002) in the error window, which should open
the following webpage in your browser (see Figure 20).

![](./assets/module-3-writing-c-code/media/image19.jpg){width="6.103888888888889in"
height="1.46875in"}

**Figure 20.** Detailed information about the error.

In C#, the termination of a code statement in a line is indicated via
semicolon. At the end of each code line, we have to add a semicolon.
Please add a semicolon to fix the error as indicated in Figure 21 and
run your application again.

![](./assets/module-3-writing-c-code/media/image20.jpg){width="4.365277777777778in"
height="0.7846948818897638in"}

**Figure 21.** Final code to change the label text.

The form should be loaded with the correct message displayed in the
label (see Figure 22).

> ![](./assets/module-3-writing-c-code/media/image21.jpg){width="3.652083333333333in"
> height="2.4944444444444445in"}

**Figure 22.** Form after running the application.

# 4) Implementing button click events 

Clicking a button is a very typical and common user action performed in
forms. We will add two buttons to our form. These buttons will serve for
changing the label text when clicked by the user. First, please add two
buttons under the label and change their Text and Name properties as
suggested in Figure 23.

> ![](./assets/module-3-writing-c-code/media/image211.jpg)

**Figure 23.** Adding two buttons to the form.

Our goal is to change the label text to "*Text has been changed*." when
the first button (btn_changeText) is clicked, and to change the label
text to "*Initial Message*" when the second button (btn_resetText) is
clicked.

  ---------------------------------------------
  Name of the click event handler The code goes
  here.
  ---------------------------------------------

  ---------------------------------------------

![](./assets/module-3-writing-c-code/media/image23.jpg){width="4.436805555555556in"
height="1.451111111111111in"}All code that we want to execute when a
button is clicked should go inside that button's click event handler.
That means we need to first create a click event handler for each button
separately. To do that, please first double-click on the btn_changeText
button, which should open the source code file with the click event
handler created (just under the frm_main_Load form load handler), as
seen in Figure 24 below.

**Figure 24.** Click event handler for btn_changeText.

The name of the click event handler, btn_changeText_Click, is
automatically created by appending the \_Click keyword to the end of the
button name (btn_changeText). Any code we want to execute should go
between the opening and closing braces { }. We will add the code to
change the text of the label (as we did for the form load event
handler). See Figure 25.

![](./assets/module-3-writing-c-code/media/image24.jpg){width="4.469166666666666in"
height="0.6798611111111111in"}

**Figure 25.** Adding code inside btn_changeText_Click to change the
label text.

We will repeat the same process for the btn_resetText button. Please
press *Shift+F7* to switch to the design view, and then double click on
the btn_resetText button to add a click event handler. Then write the
necessary code to change the label text as intended. Figure 26 shows the
implementation of the btn_resetText_Click click event handler.

![](./assets/module-3-writing-c-code/media/image25.jpg){width="4.388888888888889in"
height="0.7291108923884514in"}

**Figure 26.** Adding code inside btn_resetText_Click to change the
label text.

The content of the complete source code file is shown in Figure 27.

![](./assets/module-3-writing-c-code/media/image26.jpg){width="4.965138888888889in"
height="5.238194444444445in"}

**Figure 27.** The complete source code file.

Please press Control+F5 key combination to test your application. First,
"*Form is loaded:)*" text should be displayed as seen in Figure 28.

> ![](./assets/module-3-writing-c-code/media/image27.jpg){width="3.5130555555555554in"
> height="2.2465277777777777in"}

**Figure 28.** The form is loaded for the first time.

Then, please click the *Change Text* button on the left. The label text
should change to "*Text has been changed*." as shown in Figure 29 below.

> ![](./assets/module-3-writing-c-code/media/image28.jpg){width="3.7911111111111113in"
> height="2.4243055555555557in"}

**Figure 29.** Form view after the Change Text button is clicked.

Now, please click on *Reset Text* button. The label text should change
to "*Initial Message"* as you can see in Figure 30.

> ![](./assets/module-3-writing-c-code/media/image29.jpg){width="3.7845833333333334in"
> height="2.420138888888889in"}

**Figure 30.** Form view after the Reset Text button is clicked.

# 4) Adding comments to explain your code 

A very important practice in coding is to add comments in your code to
explain your code and the programming logic. These comments are quite
necessary in particular when you write long and complex code in more
comprehensive projects. It is very normal that you forget about code
that you wrote a week or month ago. If you have added proper
explanations within the code, they will remind you about what you were
tyring to do with each specific line or block of code. These comments
are of more importance if you work in collaborative projects where your
peers need to make sense of your code with minimum effort. In runtime,
the compiler ignores the comments since they are not part of your
programming logic but just some descriptive text.

In C# you can add two main types of comments. First one is line
comments, which starts with two forward slashes (//). After //, whatever
you type in the same line is a comment and marked with green colour.
Figure 31 shows two examples of line comment.

![](./assets/module-3-writing-c-code/media/image290.jpg)

**Figure 31.** Adding line comments.

As seen in Figure 31, line comments can start in a new line like
**\[1\]** or they can be added to the end of a code line like **\[2\]**.

Another common type is the block comments, which consist of several
lines of comments as its names suggests (although it also can have only
one line of comment). The beginning of block comments is marked with /\*
(a forward slash followed by an asterisk) and the end of block comments
is marked with \*/ (an asterisk followed by a forward slash). Figure 32
shows an example of block comment.

> ![](./assets/module-3-writing-c-code/media/image31.jpg){width="3.6234722222222224in"
> height="1.5069444444444444in"}

**Figure 32.** Adding block comments.

# 5) Using textboxes to obtain user input 

Often in our applications we need input from users. These inputs can be
also called user data and they can be collected through various form
controls (quite similar to the web interfaces), such as textboxes,
dropdown lists, checkboxes, and radio buttons. Once the users submit
their data, we need to perform some operations on them depending on our
goal (e.g., registering a user, computing an average score).

One common way to collect user data is to ask users enter some free text
inside a text field. In Windows forms, these text fields are added using
a **Textbox** control. Before continuing with the rest of the chapter,
please create a new application, called *Module3_2*.

Textbox control is located under the *Common Windows Forms* section of
the toolbox. Just to try out, please place a textbox control inside the
form as shown in Figure 33 below.

> ![](./assets/module-3-writing-c-code/media/image310.jpg)

In the design view, it is not allowed to type into the textbox. Press
Control+F5 to run your application. As shown in Figure 34, now in
runtime you should be able to enter some text inside the textbox.

![](./assets/module-3-writing-c-code/media/image33.jpg){width="4.54125in"
height="3.6152777777777776in"}

**Figure 34.** Typing some text inside the textbox in runtime.

But how do we access (during coding) the value typed inside the textbox?
We use its **Text** property. Anything typed inside the textbox is
stored in its Text property.

In this project we will program an application which prints users' full
name based on the first name and last name provided. Delete the textbox
and add three labels with three adjacent textboxes as seen in Figure 35.
Just under them, please add a button control. Update the text and name
properties of all controls as suggested in the figure.

> ![](./assets/module-3-writing-c-code/media/image330.jpg)

**Figure 35.** Form design of the application to print full name.

You might have noticed that the third textbox, named txt_fullName, has a
different background colour than the other two textboxes. This is
because the **ReadOnly** property of txt_fullName is set to True. We
made this change because txt_fullName will only serve for displaying a
text and therefore allowing users to change its content is not
necessary. The background colour of read-only textboxes is automatically
set to grey (although it can be changed).

Our goal is that when btn_printFullName is clicked, the text entered
into txt_firstName and txt_lastName should be merged and displayed
together in txt_fullName. As you may guess, we need to create a click
event handler for btn_printFullName, and then write all necessary code
inside this handler. To automatically create the handler, please double
click on btn_printFullName. You should obtain the following screen shown
in Figure 36.

![](./assets/module-3-writing-c-code/media/image35.jpg){width="5.179027777777778in"
height="1.7722222222222221in"}

**Figure 36.** Click event handler for btn_printFullName.

Your source code file should have the using statements at the top. These
are cut off in Figure 36 to save space.

# 6) Merging the first name and last name 

To merge two (or more) strings, we can use the plus (**+**) operator.
For example, \"Visual\" + \" \" +

\"C#\" would print \"Visual C#\". To merge the first name (typed into
txt_firstName) and last name (typed into txt_lastName), we can also use
the **+** operator.

You may remember that we use the **Text** property to obtain the
contents of a textbox. We use the same property also for changing the
content of a textbox. What we need to do is merge the first name and
last name values entered by the user (i.e., txt_firstName.Text + \" \" +
txt_lastName.Text) to obtain the full name. Next, we need to assign this
merged string to the **Text** property of the txt_fullName control so
that it can display the full name. Complete code is provided in Figure
37.

![](./assets/module-3-writing-c-code/media/image36.jpg){width="5.154722222222222in"
height="1.9319444444444445in"}

**Figure 37.** Complete code for printing full name.

Please run the application (Ctrl+F5). When you click the Print Full Name
button, the full name will be displayed based on what you typed into
first name and last name fields. Figure 38 shows an example.

> ![](./assets/module-3-writing-c-code/media/image37.jpg){width="2.5720833333333335in"
> height="2.136111111111111in"}

**Figure 38.** Printing the full name of the user.

Please upload *Module3_1* and *Module3_2* project folders as separate
zip files to OdtuClass.
