using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.BD.Models;

public partial class EcommerceDbContext : DbContext
{
    public EcommerceDbContext()
    {
    }

    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<CarritoItem> CarritoItems { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Colore> Colores { get; set; }

    public virtual DbSet<Cupone> Cupones { get; set; }

    public virtual DbSet<CuponesUsuario> CuponesUsuarios { get; set; }

    public virtual DbSet<Direccione> Direcciones { get; set; }

    public virtual DbSet<Inventario> Inventarios { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<PedidoDetalle> PedidoDetalles { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<ProductoImagene> ProductoImagenes { get; set; }

    public virtual DbSet<ProductoTallaColor> ProductoTallaColors { get; set; }

    public virtual DbSet<PromocionCategorium> PromocionCategoria { get; set; }

    public virtual DbSet<PromocionProducto> PromocionProductos { get; set; }

    public virtual DbSet<Promocione> Promociones { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Talla> Tallas { get; set; }

    public virtual DbSet<Temporada> Temporadas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<UsuarioRole> UsuarioRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=mssql-188335-0.cloudclusters.net,13026;Initial Catalog=EcommerceRopa;Persist Security Info=False;User ID=andres;Password=Soypipe23@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.CarritoId).HasName("PK__Carrito__778D586B8FBA0D5B");

            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Usu).WithMany(p => p.Carritos).HasConstraintName("FK__Carrito__UsuId__628FA481");
        });

        modelBuilder.Entity<CarritoItem>(entity =>
        {
            entity.HasKey(e => e.CarritoItemId).HasName("PK__CarritoI__BFC8805A829FC777");

            entity.Property(e => e.FechaAgregado).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Carrito).WithMany(p => p.CarritoItems).HasConstraintName("FK__CarritoIt__Carri__6754599E");

            entity.HasOne(d => d.ProductoTallaColor).WithMany(p => p.CarritoItems)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CarritoIt__Produ__68487DD7");
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CatId).HasName("PK__Categori__6A1C8AFA0AEE523A");

            entity.Property(e => e.CatActivo).HasDefaultValue(true);
            entity.Property(e => e.CatFechaCreacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.CatOrden).HasDefaultValue(0);

            entity.HasOne(d => d.CatPadre).WithMany(p => p.InverseCatPadre).HasConstraintName("FK__Categoria__CatPa__4AB81AF0");
        });

        modelBuilder.Entity<Colore>(entity =>
        {
            entity.HasKey(e => e.ColorId).HasName("PK__Colores__8DA7674D382273B8");
        });

        modelBuilder.Entity<Cupone>(entity =>
        {
            entity.HasKey(e => e.CuponId).HasName("PK__Cupones__C43568971EF4D55E");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.UsosRealizados).HasDefaultValue(0);
        });

        modelBuilder.Entity<CuponesUsuario>(entity =>
        {
            entity.HasKey(e => e.CuponUsuarioId).HasName("PK__CuponesU__FD77A52A8E26A747");

            entity.Property(e => e.Usado).HasDefaultValue(false);

            entity.HasOne(d => d.Cupon).WithMany(p => p.CuponesUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CuponesUs__Cupon__7B5B524B");

            entity.HasOne(d => d.Usu).WithMany(p => p.CuponesUsuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CuponesUs__UsuId__7C4F7684");
        });

        modelBuilder.Entity<Direccione>(entity =>
        {
            entity.HasKey(e => e.DirId).HasName("PK__Direccio__E364B44DD21DB50F");

            entity.Property(e => e.DirEsPrincipal).HasDefaultValue(false);
            entity.Property(e => e.DirFechaCreacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Usu).WithMany(p => p.Direcciones).HasConstraintName("FK__Direccion__UsuId__44FF419A");
        });

        modelBuilder.Entity<Inventario>(entity =>
        {
            entity.HasKey(e => e.InventarioId).HasName("PK__Inventar__FB8A24D7897BAD2F");

            entity.Property(e => e.FechaMovimiento).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ProductoTallaColor).WithMany(p => p.Inventarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventari__Produ__123EB7A3");

            entity.HasOne(d => d.Usu).WithMany(p => p.Inventarios).HasConstraintName("FK__Inventari__UsuId__1332DBDC");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__Logs__5E54864839C75387");

            entity.Property(e => e.Fecha).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Usu).WithMany(p => p.Logs).HasConstraintName("FK__Logs__UsuId__0E6E26BF");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.PedidoId).HasName("PK__Pedidos__09BA1430F18982AB");

            entity.Property(e => e.Estado).HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaPedido).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.DireccionEnvio).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pedidos__Direcci__6E01572D");

            entity.HasOne(d => d.Usu).WithMany(p => p.Pedidos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pedidos__UsuId__6D0D32F4");
        });

        modelBuilder.Entity<PedidoDetalle>(entity =>
        {
            entity.HasKey(e => e.PedidoDetalleId).HasName("PK__PedidoDe__12156C4360AF150D");

            entity.Property(e => e.Subtotal).HasComputedColumnSql("([Cantidad]*[PrecioUnitario])", true);

            entity.HasOne(d => d.Pedido).WithMany(p => p.PedidoDetalles).HasConstraintName("FK__PedidoDet__Pedid__71D1E811");

            entity.HasOne(d => d.ProductoTallaColor).WithMany(p => p.PedidoDetalles)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PedidoDet__Produ__72C60C4A");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProId).HasName("PK__Producto__6202959028DA942C");

            entity.Property(e => e.ProActivo).HasDefaultValue(true);
            entity.Property(e => e.ProFechaCreacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.ProCategoria).WithMany(p => p.Productos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productos__ProCa__4F7CD00D");
        });

        modelBuilder.Entity<ProductoImagene>(entity =>
        {
            entity.HasKey(e => e.ImgId).HasName("PK__Producto__352F54F3692A50D2");

            entity.Property(e => e.ImgFechaCreacion).HasDefaultValueSql("(getdate())");
            entity.Property(e => e.ImgOrden).HasDefaultValue(0);

            entity.HasOne(d => d.Pro).WithMany(p => p.ProductoImagenes).HasConstraintName("FK__ProductoI__ProId__5441852A");
        });

        modelBuilder.Entity<ProductoTallaColor>(entity =>
        {
            entity.HasKey(e => e.ProductoTallaColorId).HasName("PK__Producto__2835C1C557274C27");

            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");

            entity.HasOne(d => d.Color).WithMany(p => p.ProductoTallaColors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductoT__Color__5EBF139D");

            entity.HasOne(d => d.Pro).WithMany(p => p.ProductoTallaColors).HasConstraintName("FK__ProductoT__ProId__5CD6CB2B");

            entity.HasOne(d => d.Talla).WithMany(p => p.ProductoTallaColors)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductoT__Talla__5DCAEF64");
        });

        modelBuilder.Entity<PromocionCategorium>(entity =>
        {
            entity.HasKey(e => e.PromocionCategoriaId).HasName("PK__Promocio__3EEEE3A35275B546");

            entity.HasOne(d => d.Cat).WithMany(p => p.PromocionCategoria).HasConstraintName("FK__Promocion__CatId__06CD04F7");

            entity.HasOne(d => d.Promocion).WithMany(p => p.PromocionCategoria).HasConstraintName("FK__Promocion__Promo__05D8E0BE");
        });

        modelBuilder.Entity<PromocionProducto>(entity =>
        {
            entity.HasKey(e => e.PromocionProductoId).HasName("PK__Promocio__64E32947016FA81A");

            entity.HasOne(d => d.Pro).WithMany(p => p.PromocionProductos).HasConstraintName("FK__Promocion__ProId__02FC7413");

            entity.HasOne(d => d.Promocion).WithMany(p => p.PromocionProductos).HasConstraintName("FK__Promocion__Promo__02084FDA");
        });

        modelBuilder.Entity<Promocione>(entity =>
        {
            entity.HasKey(e => e.PromocionId).HasName("PK__Promocio__2DA61D9D4735AF97");

            entity.Property(e => e.Activo).HasDefaultValue(true);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Roles__F92302F12EBB82C2");
        });

        modelBuilder.Entity<Talla>(entity =>
        {
            entity.HasKey(e => e.TallaId).HasName("PK__Tallas__9BF1376DEBBBC879");

            entity.Property(e => e.TalOrdenVisualizacion).HasDefaultValue(0);
        });

        modelBuilder.Entity<Temporada>(entity =>
        {
            entity.HasKey(e => e.TemporadaId).HasName("PK__Temporad__0B8A4EBC1B71C37C");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaCreacion).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuId).HasName("PK__Usuarios__68526383AF46104B");

            entity.Property(e => e.UsuEstadoCuenta).HasDefaultValue("Activo");
            entity.Property(e => e.UsuFechaRegistro).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<UsuarioRole>(entity =>
        {
            entity.HasKey(e => e.UsuarioRolId).HasName("PK__UsuarioR__C869CDCA07EEE733");

            entity.HasOne(d => d.Rol).WithMany(p => p.UsuarioRoles).HasConstraintName("FK__UsuarioRo__RolId__403A8C7D");

            entity.HasOne(d => d.Usu).WithMany(p => p.UsuarioRoles).HasConstraintName("FK__UsuarioRo__UsuId__3F466844");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
