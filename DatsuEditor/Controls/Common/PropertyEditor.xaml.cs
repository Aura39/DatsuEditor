using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DatsuEditor.Common;
using Microsoft.VisualBasic.FileIO;

namespace DatsuEditor.Controls.Common
{
    /// <summary>
    /// Interaction logic for PropertyEditor.xaml
    /// </summary>
    public partial class PropertyEditor : UserControl
    {
        object TargetObject;
        List<PropertyInfo> Properties;
        public PropertyEditor()
        {
            InitializeComponent();
        }

        public void SetTarget(object NewTarget)
        {
            if (NewTarget == null)
            {
                PropertyGrid.Children.Clear();
                return;
            }
            Properties = NewTarget.GetType().GetProperties().Where(x => Attribute.IsDefined(x, typeof(EditablePropertyAttribute))).ToList();
            TargetObject = NewTarget;
            RefreshProperties();
        }

        public FrameworkElement GetEditor(PropertyInfo Prop)
        {
            if (Prop.PropertyType == typeof(string))
            { 
                var Value = new TextBox();
                Value.TextChanged += (s, e) => Prop.SetValue(TargetObject, Value.Text);
                Value.Text = (string)Prop.GetValue(TargetObject);
                return Value;
            }
            if (Prop.PropertyType == typeof(bool))
            {
                var Value = new CheckBox();
                Value.Checked += (s, e) => Prop.SetValue(TargetObject, Value.IsChecked);
                Value.IsChecked = (bool)Prop.GetValue(TargetObject);
                Value.HorizontalAlignment = HorizontalAlignment.Center;
                return Value;
            }

            TextBlock Dummy = new();
            Dummy.Text = "[Not editable]";
            Dummy.HorizontalAlignment = HorizontalAlignment.Center;
            return Dummy;
        }

        public void RefreshProperties()
        {
            PropertyGrid.Children.Clear();
            PropertyGrid.RowDefinitions.Clear();
            int RowOffset = 0;
            for (int i = 0; i < Properties.Count; i++)
            {
                {
                    var row = new RowDefinition();
                    row.MinHeight = 24;
                    row.Height = GridLength.Auto;
                    PropertyGrid.RowDefinitions.Add(row);
                }
                var Prop = Properties[i];
                var Attrib = (EditablePropertyAttribute)Prop.GetCustomAttribute(typeof(EditablePropertyAttribute));

                if (Prop.PropertyType.IsGenericType && Prop.PropertyType.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                {
                    var a = Prop.PropertyType.GetGenericArguments();
                    if (a[0] == typeof(string) &&
                        a[1] == typeof(string))
                    {
                        var Dict = (Dictionary<string, string>)Prop.GetValue(TargetObject);
                        {
                            {
                                var row = new RowDefinition();
                                row.MinHeight = 24;
                                row.Height = GridLength.Auto;
                                PropertyGrid.RowDefinitions.Add(row);
                            }
                            Separator s = new();
                            PropertyGrid.Children.Add(s);
                            Grid.SetRow(s, i + RowOffset);
                            Grid.SetColumn(s, 0);
                            Grid.SetColumnSpan(s, 2);
                        }
                        RowOffset++;
                        TextBlock Header = new();
                        Header.Text = Attrib.DisplayedName == "" ? Prop.Name : Attrib.DisplayedName;
                        Header.HorizontalAlignment = HorizontalAlignment.Center;
                        Header.VerticalAlignment = VerticalAlignment.Center;
                        PropertyGrid.Children.Add(Header);
                        Grid.SetRow(Header, i + RowOffset);
                        Grid.SetColumn(Header, 0);
                        Grid.SetColumnSpan(Header, 2);
                        foreach (var KV in Dict)
                        {
                            RowOffset++;
                            {
                                var row = new RowDefinition();
                                row.MinHeight = 24;
                                row.Height = GridLength.Auto;
                                PropertyGrid.RowDefinitions.Add(row);
                            }
                            TextBlock Text = new();
                            Text.Text = KV.Key;
                            Text.VerticalAlignment = VerticalAlignment.Center;
                            PropertyGrid.Children.Add(Text);
                            Grid.SetRow(Text, i + RowOffset);
                            Grid.SetColumn(Text, 0);

                            FrameworkElement Value = GetMapValueEditor(Dict, KV);
                            Value.VerticalAlignment = VerticalAlignment.Center;
                            PropertyGrid.Children.Add(Value);
                            Grid.SetRow(Value, i + RowOffset);
                            Grid.SetColumn(Value, 1);
                        }
                        RowOffset++;
                        {
                            {
                                var row = new RowDefinition();
                                row.MinHeight = 24;
                                row.Height = GridLength.Auto;
                                PropertyGrid.RowDefinitions.Add(row);
                            }
                            Separator s = new();
                            PropertyGrid.Children.Add(s);
                            Grid.SetRow(s, i + RowOffset);
                            Grid.SetColumn(s, 0);
                            Grid.SetColumnSpan(s, 2);
                        }
                    }
                    else
                    {
                        throw new NotSupportedException("PropertyEditor tried to generate a dictionary that is not Dictionary<string, string>.");
                    }
                }
                else
                {
                    TextBlock Text = new();
                    Text.Text = Attrib.DisplayedName == "" ? Prop.Name : Attrib.DisplayedName;
                    Text.VerticalAlignment = VerticalAlignment.Center;
                    PropertyGrid.Children.Add(Text);
                    Grid.SetRow(Text, i + RowOffset);
                    Grid.SetColumn(Text, 0);

                    FrameworkElement Value = GetEditor(Prop);
                    Value.VerticalAlignment = VerticalAlignment.Center;
                    PropertyGrid.Children.Add(Value);
                    Grid.SetRow(Value, i + RowOffset);
                    Grid.SetColumn(Value, 1);
                }
            }
        }
    }
}
