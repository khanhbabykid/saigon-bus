//
//
//  Generated by StarUML(tm) C# Add-In
//
//  @ Project : Untitled
//  @ File Name : Route.cs
//  @ Date : 27/11/2011
//  @ Author : 
//
//


/// <summary></summary>
/// <summary></summary>
using System;
namespace ITS.Domain.Entities
{
    public class BusRoute
    {
        public Guid RouteID;
        public String RouteName;

        public override bool Equals(object obj)
        {
            return this.RouteID.Equals(((BusRoute)obj).RouteID);
        }
    }
}