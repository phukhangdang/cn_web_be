using Microsoft.EntityFrameworkCore;

namespace CN_WEB.Core.Model
{
    public partial class SysDbContext : DbContext
    {
        private DbContextOptions<SysDbReadContext> _optionsRead;
        private DbContextOptions<SysDbWriteContext> _optionsWrite;

        public SysDbContext(DbContextOptions<SysDbReadContext> options)
        {
            _optionsRead = options;
        }

        public SysDbContext(DbContextOptions<SysDbWriteContext> options)
        {
            _optionsWrite = options;
        }
    }

    public class SysDbReadContext : SysDbContext
    {
        public SysDbReadContext(DbContextOptions<SysDbReadContext> options)
            : base(options)
        {
        }
    }

    public class SysDbWriteContext : SysDbContext
    {
        public SysDbWriteContext(DbContextOptions<SysDbWriteContext> options)
            : base(options)
        {
        }
    }
}
