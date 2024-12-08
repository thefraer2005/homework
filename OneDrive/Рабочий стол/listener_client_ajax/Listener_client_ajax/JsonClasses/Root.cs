using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Listener_client_ajax;
public class Root
{
    public string Description { get; set; }
    public string Headline { get; set; }
    public string Link { get; set; }
    public DateTime Published { get; set; }
    public string Premium { get; set; }
    public DateTime LastModified { get; set; }
    public List<Category> Categories { get; set; }
}
