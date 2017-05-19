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




}
