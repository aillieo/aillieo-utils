#include <vector>

#pragma once

using std::vector;

namespace ExtGeom 
{
	struct Point2
	{
		double x;
		double y;
		Point2(double _x, double _y):x(_x),y(_y){}
	};

	struct Point3
	{
		double x;
		double y;
		double z;
		Point3(double _x, double _y, double _z):x(_x),y(_y),z(_z){}
	};


	struct Line2
	{
		Point2 p1;
		Point2 p2;
		Line2(Point2 _p1, Point2 _p2):p1(_p1),p2(_p2){}
		float length(){return 100.0f;}
	};

}
