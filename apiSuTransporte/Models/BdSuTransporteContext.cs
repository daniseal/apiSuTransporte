using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace apiSuTransporte.Models;

public partial class BdSuTransporteContext : DbContext
{
    public BdSuTransporteContext()
    {
    }

    public BdSuTransporteContext(DbContextOptions<BdSuTransporteContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Camion> Camions { get; set; }

    public virtual DbSet<Camionero> Camioneros { get; set; }

    public virtual DbSet<Ciudad> Ciudads { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Departamento> Departamentos { get; set; }

    public virtual DbSet<DetalleGuium> DetalleGuia { get; set; }

    public virtual DbSet<DirCamionero> DirCamioneros { get; set; }

    public virtual DbSet<DirCliente> DirClientes { get; set; }

    public virtual DbSet<DirEmpleado> DirEmpleados { get; set; }

    public virtual DbSet<Empleado> Empleados { get; set; }

    public virtual DbSet<Entrega> Entregas { get; set; }

    public virtual DbSet<EstadoEntrega> EstadoEntregas { get; set; }

    public virtual DbSet<EstadoGuium> EstadoGuia { get; set; }

    public virtual DbSet<EstadoServicio> EstadoServicios { get; set; }

    public virtual DbSet<GuiaDespacho> GuiaDespachos { get; set; }

    public virtual DbSet<Marca> Marcas { get; set; }

    public virtual DbSet<Sede> Sedes { get; set; }

    public virtual DbSet<Servicio> Servicios { get; set; }

    public virtual DbSet<TelCamionero> TelCamioneros { get; set; }

    public virtual DbSet<TelCliente> TelClientes { get; set; }

    public virtual DbSet<TelEmpleado> TelEmpleados { get; set; }

    public virtual DbSet<TipoCamion> TipoCamions { get; set; }

    public virtual DbSet<TipoDocumento> TipoDocumentos { get; set; }

    public virtual DbSet<TipoProducto> TipoProductos { get; set; }

    public virtual DbSet<TipoTelefono> TipoTelefonos { get; set; }

    public virtual DbSet<UsuarioSistema> UsuarioSistemas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=bdSuTransporte;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Camion>(entity =>
        {
            entity.HasKey(e => e.IdCamion).HasName("PK__Camion__B706E776E78D4D56");

            entity.ToTable("Camion");

            entity.HasIndex(e => e.NumeroChasis, "UQ_Camion_NumeroChasis").IsUnique();

            entity.HasIndex(e => e.NumeroMotor, "UQ_Camion_NumeroMotor").IsUnique();

            entity.HasIndex(e => e.Placa, "UQ_Camion_Placa").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Estado).HasMaxLength(60);
            entity.Property(e => e.Modelo).HasMaxLength(50);
            entity.Property(e => e.NumeroChasis).HasMaxLength(60);
            entity.Property(e => e.NumeroMotor).HasMaxLength(60);
            entity.Property(e => e.Placa).HasMaxLength(20);
            entity.Property(e => e.Potencia).HasMaxLength(50);

            entity.HasOne(d => d.IdMarcaNavigation).WithMany(p => p.Camions)
                .HasForeignKey(d => d.IdMarca)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Camion_Marca");

            entity.HasOne(d => d.IdSedeNavigation).WithMany(p => p.Camions)
                .HasForeignKey(d => d.IdSede)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Camion_Sede");

            entity.HasOne(d => d.IdTipoCamionNavigation).WithMany(p => p.Camions)
                .HasForeignKey(d => d.IdTipoCamion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Camion_TipoCamion");
        });

        modelBuilder.Entity<Camionero>(entity =>
        {
            entity.HasKey(e => e.IdCamionero).HasName("PK__Camioner__A7A56198B278BA53");

            entity.ToTable("Camionero");

            entity.HasIndex(e => new { e.IdTipoDocumento, e.NumeroDocumento }, "UQ_Camionero_Documento").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.CategoriaPase).HasMaxLength(20);
            entity.Property(e => e.Nombres).HasMaxLength(100);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(30);
            entity.Property(e => e.Salario).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.IdTipoDocumentoNavigation).WithMany(p => p.Camioneros)
                .HasForeignKey(d => d.IdTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Camionero_TipoDocumento");
        });

        modelBuilder.Entity<Ciudad>(entity =>
        {
            entity.HasKey(e => e.IdCiudad).HasName("PK__Ciudad__D4D3CCB03BD6538B");

            entity.ToTable("Ciudad");

            entity.HasIndex(e => new { e.IdDepartamento, e.Nombre }, "UQ_Ciudad").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(80);

            entity.HasOne(d => d.IdDepartamentoNavigation).WithMany(p => p.Ciudads)
                .HasForeignKey(d => d.IdDepartamento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Ciudad_Departamento");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Cliente__D5946642B01728C1");

            entity.ToTable("Cliente");

            entity.HasIndex(e => new { e.IdTipoDocumento, e.NumeroDocumento }, "UQ_Cliente_Documento").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.NombreCompleto).HasMaxLength(150);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(30);

            entity.HasOne(d => d.IdTipoDocumentoNavigation).WithMany(p => p.Clientes)
                .HasForeignKey(d => d.IdTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Cliente_TipoDocumento");
        });

        modelBuilder.Entity<Departamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento).HasName("PK__Departam__787A433D50DB4C0A");

            entity.ToTable("Departamento");

            entity.HasIndex(e => e.Nombre, "UQ_Departamento_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(80);
        });

        modelBuilder.Entity<DetalleGuium>(entity =>
        {
            entity.HasKey(e => e.IdDetalleGuia).HasName("PK__DetalleG__CC00AB8175F31311");

            entity.HasIndex(e => e.IdEntrega, "UQ_DetalleGuia_Entrega").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.FechaEstado).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Observaciones).HasMaxLength(250);

            entity.HasOne(d => d.IdEntregaNavigation).WithOne(p => p.DetalleGuium)
                .HasForeignKey<DetalleGuium>(d => d.IdEntrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleGuia_Entrega");

            entity.HasOne(d => d.IdEstadoEntregaNavigation).WithMany(p => p.DetalleGuia)
                .HasForeignKey(d => d.IdEstadoEntrega)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleGuia_EstadoEntrega");

            entity.HasOne(d => d.IdGuiaDespachoNavigation).WithMany(p => p.DetalleGuia)
                .HasForeignKey(d => d.IdGuiaDespacho)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DetalleGuia_GuiaDespacho");
        });

        modelBuilder.Entity<DirCamionero>(entity =>
        {
            entity.HasKey(e => e.IdDirCamionero).HasName("PK__DirCamio__9EF552091F58F42F");

            entity.ToTable("DirCamionero");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Direccion).HasMaxLength(150);

            entity.HasOne(d => d.IdCamioneroNavigation).WithMany(p => p.DirCamioneros)
                .HasForeignKey(d => d.IdCamionero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirCamionero_Camionero");

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.DirCamioneros)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirCamionero_Ciudad");
        });

        modelBuilder.Entity<DirCliente>(entity =>
        {
            entity.HasKey(e => e.IdDirCliente).HasName("PK__DirClien__E257FBE444AF6727");

            entity.ToTable("DirCliente");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Direccion).HasMaxLength(150);

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.DirClientes)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirCliente_Ciudad");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.DirClientes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirCliente_Cliente");
        });

        modelBuilder.Entity<DirEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdDirEmpleado).HasName("PK__DirEmple__0BF7D87C49EA2D7B");

            entity.ToTable("DirEmpleado");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Direccion).HasMaxLength(150);

            entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.DirEmpleados)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirEmpleado_Ciudad");

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.DirEmpleados)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DirEmpleado_Empleado");
        });

        modelBuilder.Entity<Empleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado).HasName("PK__Empleado__CE6D8B9E792BA59B");

            entity.ToTable("Empleado");

            entity.HasIndex(e => new { e.IdTipoDocumento, e.NumeroDocumento }, "UQ_Empleado_Documento").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Apellidos).HasMaxLength(100);
            entity.Property(e => e.Cargo).HasMaxLength(80);
            entity.Property(e => e.Nombres).HasMaxLength(100);
            entity.Property(e => e.NumeroDocumento).HasMaxLength(30);

            entity.HasOne(d => d.IdTipoDocumentoNavigation).WithMany(p => p.Empleados)
                .HasForeignKey(d => d.IdTipoDocumento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Empleado_TipoDocumento");
        });

        modelBuilder.Entity<Entrega>(entity =>
        {
            entity.HasKey(e => e.IdEntrega).HasName("PK__Entrega__C852F5534ABB7C24");

            entity.ToTable("Entrega");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ApellidoDestinatario).HasMaxLength(100);
            entity.Property(e => e.Contenido).HasMaxLength(150);
            entity.Property(e => e.Costo).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.Dimensiones).HasMaxLength(80);
            entity.Property(e => e.DireccionDestino).HasMaxLength(150);
            entity.Property(e => e.NombreDestinatario).HasMaxLength(100);
            entity.Property(e => e.Observaciones).HasMaxLength(600);
            entity.Property(e => e.PesoKg).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TelefonoDestinatario).HasMaxLength(30);
            entity.Property(e => e.ValorDeclarado).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.IdCiudadDestinoNavigation).WithMany(p => p.Entregas)
                .HasForeignKey(d => d.IdCiudadDestino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Entrega_Ciudad");

            entity.HasOne(d => d.IdServicioNavigation).WithMany(p => p.Entregas)
                .HasForeignKey(d => d.IdServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Entrega_Servicio");

            entity.HasOne(d => d.IdTipoProductoNavigation).WithMany(p => p.Entregas)
                .HasForeignKey(d => d.IdTipoProducto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Entrega_TipoProducto");
        });

        modelBuilder.Entity<EstadoEntrega>(entity =>
        {
            entity.HasKey(e => e.IdEstadoEntrega).HasName("PK__EstadoEn__99D65F704F4C66B2");

            entity.ToTable("EstadoEntrega");

            entity.HasIndex(e => e.Nombre, "UQ_EstadoEntrega_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(60);
        });

        modelBuilder.Entity<EstadoGuium>(entity =>
        {
            entity.HasKey(e => e.IdEstadoGuia).HasName("PK__EstadoGu__EE6688028CBA0287");

            entity.HasIndex(e => e.Nombre, "UQ_EstadoGuia_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(60);
        });

        modelBuilder.Entity<EstadoServicio>(entity =>
        {
            entity.HasKey(e => e.IdEstadoServicio).HasName("PK__EstadoSe__6E13DB5455A6D0FF");

            entity.ToTable("EstadoServicio");

            entity.HasIndex(e => e.Nombre, "UQ_EstadoServicio_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(60);
        });

        modelBuilder.Entity<GuiaDespacho>(entity =>
        {
            entity.HasKey(e => e.IdGuiaDespacho).HasName("PK__GuiaDesp__90A056EC7351B3BB");

            entity.ToTable("GuiaDespacho");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CostoTotal).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.FechaDistribucion).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Observaciones).HasMaxLength(600);
            entity.Property(e => e.ValorTotal).HasColumnType("decimal(12, 2)");

            entity.HasOne(d => d.IdCamionNavigation).WithMany(p => p.GuiaDespachos)
                .HasForeignKey(d => d.IdCamion)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaDespacho_Camion");

            entity.HasOne(d => d.IdCamioneroNavigation).WithMany(p => p.GuiaDespachos)
                .HasForeignKey(d => d.IdCamionero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaDespacho_Camionero");

            entity.HasOne(d => d.IdCiudadDestinoNavigation).WithMany(p => p.GuiaDespachos)
                .HasForeignKey(d => d.IdCiudadDestino)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaDespacho_CiudadDestino");

            entity.HasOne(d => d.IdEmpleadoDespachaNavigation).WithMany(p => p.GuiaDespachoIdEmpleadoDespachaNavigations)
                .HasForeignKey(d => d.IdEmpleadoDespacha)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaDespacho_EmpleadoDespacha");

            entity.HasOne(d => d.IdEmpleadoRecibeNavigation).WithMany(p => p.GuiaDespachoIdEmpleadoRecibeNavigations)
                .HasForeignKey(d => d.IdEmpleadoRecibe)
                .HasConstraintName("FK_GuiaDespacho_EmpleadoRecibe");

            entity.HasOne(d => d.IdEstadoGuiaNavigation).WithMany(p => p.GuiaDespachos)
                .HasForeignKey(d => d.IdEstadoGuia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GuiaDespacho_EstadoGuia");
        });

        modelBuilder.Entity<Marca>(entity =>
        {
            entity.HasKey(e => e.IdMarca).HasName("PK__Marca__4076A887E86E8823");

            entity.ToTable("Marca");

            entity.HasIndex(e => e.Nombre, "UQ_Marca_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Descripcion).HasMaxLength(200);
            entity.Property(e => e.Nombre).HasMaxLength(80);
        });

        modelBuilder.Entity<Sede>(entity =>
        {
            entity.HasKey(e => e.IdSede).HasName("PK__Sede__A7780DFFDB3253AF");

            entity.ToTable("Sede");

            entity.HasIndex(e => e.Nombre, "UQ_Sede_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Servicio>(entity =>
        {
            entity.HasKey(e => e.IdServicio).HasName("PK__Servicio__2DCCF9A258622AA0");

            entity.ToTable("Servicio");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.CostoServicio).HasColumnType("decimal(12, 2)");
            entity.Property(e => e.FechaServicio).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.Observaciones).HasMaxLength(250);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_Cliente");

            entity.HasOne(d => d.IdDirClienteOrigenNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdDirClienteOrigen)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_DirCliente");

            entity.HasOne(d => d.IdEmpleadoRecibeNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdEmpleadoRecibe)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_Empleado");

            entity.HasOne(d => d.IdEstadoServicioNavigation).WithMany(p => p.Servicios)
                .HasForeignKey(d => d.IdEstadoServicio)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Servicio_EstadoServicio");
        });

        modelBuilder.Entity<TelCamionero>(entity =>
        {
            entity.HasKey(e => e.IdTelCamionero).HasName("PK__TelCamio__220E43858D48838C");

            entity.ToTable("TelCamionero");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.NumeroTelefono).HasMaxLength(30);

            entity.HasOne(d => d.IdCamioneroNavigation).WithMany(p => p.TelCamioneros)
                .HasForeignKey(d => d.IdCamionero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelCamionero_Camionero");

            entity.HasOne(d => d.IdTipoTelefonoNavigation).WithMany(p => p.TelCamioneros)
                .HasForeignKey(d => d.IdTipoTelefono)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelCamionero_TipoTelefono");
        });

        modelBuilder.Entity<TelCliente>(entity =>
        {
            entity.HasKey(e => e.IdTelCliente).HasName("PK__TelClien__B7056D4EEA0BC689");

            entity.ToTable("TelCliente");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.NumeroTelefono).HasMaxLength(30);

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.TelClientes)
                .HasForeignKey(d => d.IdCliente)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelCliente_Cliente");

            entity.HasOne(d => d.IdTipoTelefonoNavigation).WithMany(p => p.TelClientes)
                .HasForeignKey(d => d.IdTipoTelefono)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelCliente_TipoTelefono");
        });

        modelBuilder.Entity<TelEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdTelEmpleado).HasName("PK__TelEmple__1491974A1E8A820C");

            entity.ToTable("TelEmpleado");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.NumeroTelefono).HasMaxLength(30);

            entity.HasOne(d => d.IdEmpleadoNavigation).WithMany(p => p.TelEmpleados)
                .HasForeignKey(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelEmpleado_Empleado");

            entity.HasOne(d => d.IdTipoTelefonoNavigation).WithMany(p => p.TelEmpleados)
                .HasForeignKey(d => d.IdTipoTelefono)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TelEmpleado_TipoTelefono");
        });

        modelBuilder.Entity<TipoCamion>(entity =>
        {
            entity.HasKey(e => e.IdTipoCamion).HasName("PK__TipoCami__331341CFB77A70CD");

            entity.ToTable("TipoCamion");

            entity.HasIndex(e => e.Nombre, "UQ_TipoCamion_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(80);
        });

        modelBuilder.Entity<TipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento).HasName("PK__TipoDocu__3AB3332F8581A0D1");

            entity.ToTable("TipoDocumento");

            entity.HasIndex(e => e.Nombre, "UQ_TipoDocumento_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<TipoProducto>(entity =>
        {
            entity.HasKey(e => e.IdTipoProducto).HasName("PK__TipoProd__A974F920C2679B74");

            entity.ToTable("TipoProducto");

            entity.HasIndex(e => e.Nombre, "UQ_TipoProducto_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(80);
        });

        modelBuilder.Entity<TipoTelefono>(entity =>
        {
            entity.HasKey(e => e.IdTipoTelefono).HasName("PK__TipoTele__6FA65CBF3C2607EF");

            entity.ToTable("TipoTelefono");

            entity.HasIndex(e => e.Nombre, "UQ_TipoTelefono_Nombre").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Nombre).HasMaxLength(50);
        });

        modelBuilder.Entity<UsuarioSistema>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__UsuarioS__5B65BF9739E7AD71");

            entity.ToTable("UsuarioSistema");

            entity.HasIndex(e => e.IdEmpleado, "UQ_UsuarioSistema_Empleado").IsUnique();

            entity.HasIndex(e => e.Usuario, "UQ_UsuarioSistema_Usuario").IsUnique();

            entity.Property(e => e.Clave).HasMaxLength(150);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.Usuario).HasMaxLength(60);

            entity.HasOne(d => d.IdEmpleadoNavigation).WithOne(p => p.UsuarioSistema)
                .HasForeignKey<UsuarioSistema>(d => d.IdEmpleado)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UsuarioSistema_Empleado");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
