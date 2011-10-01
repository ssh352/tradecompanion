/// <summary>****************************************************
/// 
/// Copyright (c) 1999-2005 S7 Software Solutions Pvt. Ltd. Software Inc.,
/// #113 Railway Parallel Road, Kumara Park West, Bangalore - 560020, India.
/// All rights reserved.
/// 
/// This software is the confidential and proprietary information
/// of S7 Software Solutions Pvt. Ltd.("Confidential Information").  You
/// shall not disclose such Confidential Information and shall use
/// it only in accordance with the terms of the license agreement
/// you entered into with S7 Software Solutions Pvt. Ltd.
/// 
/// ****************************************************
/// </summary>
using System;
namespace com.scalper.util
{


    /// <summary> This is the root interface for all files that describe configurable properties in cfg files used in S7 Software Solutions Pvt. Ltd.oft products.
    /// This interface exists for the convenience of the user/administrator who is using these documents
    /// to configure any S7 Software Solutions Pvt. Ltd.oft application.
    /// 
    /// <P>All files that implement this interface will have properties of the format "PROP_" + name, e.g. PROP_Password.
    /// Such a property describes the configurable Password property.  The documentation on that individual file will
    /// explain where and when such a property is applicable, and in which file it can be set.
    /// 
    /// <P>All boolean properties, unless otherwise specified, default to false.
    /// </summary>
    public interface ConfigurationProperties
    {
        System.String getProperty(System.String property);
    }
}