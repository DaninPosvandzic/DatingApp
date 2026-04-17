using System;

namespace API.Helpers;

public class PagingParams
{
public const int MaxPagesize =50;
public int PageNumber { get; set; }=1;
public int _pageSize=10;
public int PageSize 
{ get=>_pageSize;
  set =>_pageSize=(value>MaxPagesize)?MaxPagesize:value;
  }
}
