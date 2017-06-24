using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace _3dbinpscking
{
   public class packing
    {

            List<itembox> itembox = new List<_3dbinpscking.itembox>();
            Point3DCollection points = new Point3DCollection();
            public double Setpoint(List<itembox> pbox)
            {
            double H = 6;
            double W = 7;
            double D = 5.5;
          itembox = Clone<itembox>(pbox);
            double Lx = 0;
                double Lz = 0;
            points.Clear();
            points.Add(new Point3D(0, 0, 0));
                for (int i = 0; i < itembox.Count(); i++)
                {

                    double Itemx = itembox[i].width;
                    double Itemy = itembox[i].lenght;
                    double Itemz = itembox[i].height;
                    for (int j = 0; j < points.Count; j++)
                    {
                        var Xd = points[j].X + Itemx;
                        var Zd = points[j].Z + Itemz;

                        if (itembox[i].flag == 0)
                        {
                            if (Itemx <= H - points[j].X && Itemz <= D - points[j].Z && Itemy <= W-points[j].Y&& ifinbox(j, itembox[i]) && Xd <= Lx && Zd <= Lz)
                            {
                                itembox[i].flag = 1;
                                itembox[i].point = points[j];
                                points= updatapoints(Itemy, j, Xd, Zd);
                                break;
                            }
                            if (Lx == 0 || Lx == H)
                            {

                                if (Itemx <= H - Lx && Itemz <= D - Lz&&Itemy<=W-points[j].Y&& ifinbox(j, itembox[i]))
                                {
                                    itembox[i].flag = 1;
                                    itembox[i].point = points[j];
                                    Lz = Lz + Itemz;
                                    Lx = Itemx;
                                    points = updatapoints(Itemy, j, Xd, Zd);
                                break;
                               
                            }
                                else if (Lz < D)
                                {
                                    Lz = D;
                                    Lx = H;
                                    i = i - 1;
                                }
                            
                            }
                            else if (points[j].X == Lx && points[j].Y == 0)
                            {
                                if (Itemx <= H - Lx && Itemz <= D - Lz&&Itemy<=W-points[j].Y && ifinbox(j, itembox[i]) && points[j].Z + Itemy <= Lz)
                                {
                                    itembox[i].flag = 1;
                                    itembox[i].point = points[j];
                                    Lx = Lx + Itemx;
                                    points = updatapoints(Itemy, j, Xd, Zd);
                                break;
                                }
                            if (itembox[i].flag == 0)
                            {
                                Lx = H;
                                i = i - 1;
                            }
                        }

                        }
                   
                    }


                }
           
            double allv = 0;
            foreach (var inbox in itembox)
            {
                if (inbox.flag==1)
                {
                    allv = allv + (inbox.width * inbox.height * inbox.lenght);
                  
                }
            }
            double xom =allv/(H*W*D);//计算体积装填率
            return xom ;
            }

        private Point3DCollection updatapoints(double Itemy, int j, double Xd, double Zd)
            {
            double Px = points[j].X;
            double Py = points[j].Y;
            double Pz = points[j].Z;
            points.Add(new Point3D(Xd, Py, Pz));
            points.Add(new Point3D(Px, Py + Itemy, Pz));
            points.Add(new Point3D(Px, Py, Zd));
            points.Remove(points[j]);
            List<Point3D> a = new List<Point3D>();
            for (int i = points.Count - 1; i >= 0; i--)
            {
                if (points[i].X == Px && points[i].Z == Pz && points[i].Y < Py + Itemy)
                {
                    points.RemoveAt(i);
                }
                else if (points[i].X == Px && points[i].Y == Py && points[i].Z < Zd)
                {
                    points.RemoveAt(i);
                }
                else if (points[i].Z == Pz && points[i].Y == Py && points[i].X < Xd)
                {
                    points.RemoveAt(i);
                }
            }
            Point3DCollection newpoints = new Point3DCollection();
            IEnumerable<Point3D> sortp = points.OrderBy(points => points.X).ThenBy(points => points.Y).ThenBy(points => points.Z);
            foreach (var p in sortp)
            {
                newpoints.Add(p);
            }
            return newpoints;
        }
        public static List<T> Clone<T>(object List)
        {
            using (Stream objectStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(objectStream, List);
                objectStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(objectStream) as List<T>;
            }
        }
        bool ifinbox(int j,itembox ifbox)
        {
            double Px = points[j].X;
            double Py = points[j].Y+ifbox.height;
            double Pz = points[j].Z;
            bool result = true;
            foreach (var q in points)
            {
                if (Pz==q.Z&&Py>q.Y&&q.X>Px)
                {
                    result = false;
                    break;
                }
            }
            return result;
           
        }
        public List<itembox> getbox()
        {
            return itembox;


        }
    }
    }

