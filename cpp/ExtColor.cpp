#include "ExtColor.h"
#include <sstream>

using std::vector;




ExtColor::Color3B ExtColor::Hex2Color3B(string& hex)
{
	Color3B ret = Color3B(0, 0, 0);

	if(hex.length() == 7 && hex[0] == '#')
	{
		hex = hex.substr(1,6);
	}

	if(hex.length() == 6)
	{
		int r,g,b;
		std::istringstream(hex.substr(0,2)) >> std::hex >> r;
		std::istringstream(hex.substr(2,2)) >> std::hex >> g;
		std::istringstream(hex.substr(4,2)) >> std::hex >> b;
		ret = Color3B(r,g,b);
	}
	return ret;
}

ExtColor::Color4B ExtColor::Hex2Color4B(string& hex)
{
	Color4B ret = Color4B(0, 0, 0, 0);

	if(hex.length() == 9 && hex[0] == '#')
	{
		hex = hex.substr(1,8);
	}

	if(hex.length() == 8)
	{
		int r,g,b,a;
		std::istringstream(hex.substr(0,2)) >> std::hex >> r;
		std::istringstream(hex.substr(2,2)) >> std::hex >> g;
		std::istringstream(hex.substr(4,2)) >> std::hex >> b;
		std::istringstream(hex.substr(6,2)) >> std::hex >> a;
		ret = Color4B(r,g,b,a);
	}
	return ret;

}
