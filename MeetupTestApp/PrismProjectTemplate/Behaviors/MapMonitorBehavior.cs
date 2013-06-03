using Bing.Maps;
using MeetupTestClient.UILogic.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MeetupTestClient.Behaviors
{
    public class MapMonitorBehavior : DependencyObject
    {
        private static MapLayer _mapLayer;
        private static bool _inProcess;

        #region Position
        public static Geoposition GetPosition(DependencyObject obj)
        {
            return (Geoposition)obj.GetValue(PositionProperty);
        }

        public static void SetPosition(DependencyObject obj, Geoposition value)
        {
            obj.SetValue(PositionProperty, value);
        }

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("Position", typeof(Geoposition), typeof(MapMonitorBehavior), new PropertyMetadata(null, PositionChangedCallback));

        private static void PositionChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Map map = d as Map;

            if (map == null)
                return;

            Boolean isFirstRun = false;

            Geoposition position = map.GetValue(PositionProperty) as Geoposition;

            MapLayer layer = (from ml in map.Children
                              where ml.GetType() == typeof(MapLayer) &&
                              ((MapLayer)ml).Name == "CurrentPin"
                              select ml as MapLayer).FirstOrDefault();

            if (layer == null)
            {
                layer = new MapLayer();
                layer.Name = "CurrentPin";
                map.Children.Add(layer);
                isFirstRun = true;
            }
            else
                layer.Children.Clear();

            Pushpin pin = new Pushpin();
            pin.Text = "C";
            pin.Background = new SolidColorBrush(Colors.Green);
            Location location = new Location(position.Coordinate.Latitude, position.Coordinate.Longitude);
            layer.Children.Add(pin);
            Point anchor;
            Boolean setAnchor = map.TryLocationToPixel(location, out anchor);

            //MapShapeLayer shapeLayer = new MapShapeLayer();
            //MapPolyline polyline = new MapPolyline();
            //polyline.Locations = new LocationCollection() { new Location(position.Coordinate.Latitude - .02, position.Coordinate.Longitude + .03), new Location(position.Coordinate.Latitude - .02, position.Coordinate.Longitude - .03), new Location(position.Coordinate.Latitude + .02, position.Coordinate.Longitude - .03), new Location(position.Coordinate.Latitude + .02, position.Coordinate.Longitude + .03) };
            //polyline.Color = Windows.UI.Colors.Red;
            //polyline.Width = 3;
            //shapeLayer.Shapes.Add(polyline);
            //map.ShapeLayers.Add(shapeLayer);

            if (setAnchor)
            {
                MapLayer.SetPosition(pin, location);
                MapLayer.SetPositionAnchor(pin, anchor);
            }

            if (isFirstRun)
            {
                Point point;
                Boolean gotPoint = map.TryLocationToPixel(location, out point);
                if (gotPoint)
                {
                    SetCenterView(map, location);
                }
            }
        } 
        #endregion

        #region Groups
        public static ObservableCollection<Result> GetGroups(DependencyObject obj)
        {
            return (ObservableCollection<Result>)obj.GetValue(GroupsProperty);
        }

        public static void SetGroups(DependencyObject obj, ObservableCollection<Result> value)
        {
            obj.SetValue(GroupsProperty, value);
        }

        public static readonly DependencyProperty GroupsProperty =
            DependencyProperty.Register("Groups", typeof(ObservableCollection<Result>), typeof(MapMonitorBehavior), new PropertyMetadata(null, GroupsChangedCallback));

        private static void GroupsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Map map = d as Map;
            ObservableCollection<Result> groups = (ObservableCollection<Result>)map.GetValue(GroupsProperty);

            MapLayer layer = (from ml in map.Children
                                where ml.GetType() == typeof(MapLayer) &&
                                ((MapLayer)ml).Name == "GroupsPin"
                                select ml as MapLayer).FirstOrDefault();

            if (layer == null)
            {
                layer = new MapLayer();
                layer.Name = "GroupsPin";
                map.Children.Add(layer);
            }
            else
                layer.Children.Clear();

            groups.CollectionChanged += (s, args) =>
            {
                if (args.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
                {
                    layer.Children.Clear();
                }

                if (args.NewItems != null)
                {
                    foreach (Result @group in args.NewItems)
                    {
                        Location location = new Location(@group.lat, @group.lon);

                        Pushpin pin = (Pushpin)layer.Children.Where(child => child.GetType() == typeof(Pushpin) && 
                                      ((Location)(child as Pushpin).Tag).Latitude == location.Latitude &&
                                      ((Location)(child as Pushpin).Tag).Longitude == location.Longitude).FirstOrDefault();

                        if (pin == null)
                        {
                            pin = new Pushpin();
                            pin.Text = "1";
                            pin.Background = new SolidColorBrush(Colors.Blue);
                            pin.Tag = location;

                            pin.Tapped += (sender, args1) =>
                            {
                                var results = (ObservableCollection<Result>)map.GetValue(GroupsProperty);
                                var removals = results.Where(result => result.lat != location.Latitude ||
                                                                       result.lon != location.Longitude).ToList();


                                _inProcess = true;

                                foreach (var removal in removals)
                                {
                                    results.Remove(removal);
                                }


                                _inProcess = false;

                                layer.Children.Clear();
                                layer.Children.Add(pin);
                            };

                            layer.Children.Add(pin);
                            MapLayer.SetPosition(pin, location);
                        }
                        else
                        {
                            pin.Text = (Int32.Parse(pin.Text) + 1).ToString();
                        }

                        //StackPanel panel = new StackPanel();
                        //panel.Orientation = Orientation.Horizontal;

                        //BitmapImage bitmap = new BitmapImage();
                        //if (@group.group_photo != null)
                        //{
                        //    bitmap.UriSource = new Uri(@group.group_photo.thumb_link);
                        //}
                        //Image image = new Image();
                        //image.Source = bitmap;

                        //TextBlock textBlock = new TextBlock();
                        //textBlock.Text = @group.name;
                        //textBlock.Margin = new Thickness(10, 0, 10, 0);
                        //textBlock.VerticalAlignment = VerticalAlignment.Center;

                        //panel.Children.Add(image);
                        //panel.Children.Add(textBlock);

                        //ToolTipService.SetPlacement(pin, Windows.UI.Xaml.Controls.Primitives.PlacementMode.Top);
                        //ToolTipService.SetPlacementTarget(pin, pin);
                        //ToolTipService.SetToolTip(pin, panel);
                    }
                }
                if (args.OldItems != null)
                {
                    foreach (Result @group in args.OldItems)
                    {
                        Location location = new Location(@group.lat, @group.lon);

                        Pushpin pin;

                        pin = (Pushpin)layer.Children.Where(child => child.GetType() == typeof(Pushpin) &&
                                                            ((Location)(child as Pushpin).Tag).Latitude == location.Latitude &&
                                                            ((Location)(child as Pushpin).Tag).Longitude == location.Longitude).FirstOrDefault();

                        if(pin != null && !_inProcess)
                            layer.Children.Remove(pin);
                    }
                }
            };
        } 
        #endregion

        #region Selected Group
        public static Result GetSelectedGroup(DependencyObject obj)
        {
            return (Result)obj.GetValue(SelectedGroupProperty);
        }

        public static void SetSelectedGroup(DependencyObject obj, Result value)
        {
            obj.SetValue(SelectedGroupProperty, value);
        }

        public static readonly DependencyProperty SelectedGroupProperty =
            DependencyProperty.Register("SelectedGroup", typeof(Result), typeof(MapMonitorBehavior), new PropertyMetadata(null, SelectedGroupChangedCallback));

        private static void SelectedGroupChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Map map = d as Map;

            if (map == null)
                return;

            Result r = e.NewValue as Result;
            Location location;

            if (r != null)
            {
                location = new Location(r.lat, r.lon);
            }
            else
            {
                Geoposition currentposition = (Geoposition)map.GetValue(PositionProperty);
                location = new Location(currentposition.Coordinate.Latitude, currentposition.Coordinate.Longitude);
            }

            SetCenterView(map, location);
        }
        #endregion

        private static void SetCenterView(Map map, Location location)
        {
            map.SetView(location, 13, 0, MapAnimationDuration.Default);
        }
    }
}
