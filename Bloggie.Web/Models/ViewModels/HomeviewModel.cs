using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bloggie.Web.Models.Domain;

namespace Bloggie.Web.Models.ViewModels
{
    public class HomeviewModel
    {
      public IEnumerable<BlogPost> BlogPosts{get;set;} 
      
      public IEnumerable<Tag> Tags{get;set;}

    }
}