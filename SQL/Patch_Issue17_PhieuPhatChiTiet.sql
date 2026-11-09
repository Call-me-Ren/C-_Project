USE QLThuVien;
GO

SET XACT_ABORT ON;
BEGIN TRANSACTION;

IF EXISTS (SELECT 1 FROM dbo.PhieuPhat WHERE MaPhieuMuon IS NULL)
    THROW 51001, 'PhieuPhat has rows without MaPhieuMuon. Resolve them before applying Issue 17 migration.', 1;

IF EXISTS (SELECT 1 FROM dbo.ChiTietPhieuPhat WHERE MaSach IS NULL)
    THROW 51002, 'ChiTietPhieuPhat has rows without MaSach. Resolve them before applying Issue 17 migration.', 1;

IF COL_LENGTH('dbo.ChiTietPhieuPhat', 'MaCTPP') IS NULL
    ALTER TABLE dbo.ChiTietPhieuPhat ADD MaCTPP INT IDENTITY(1,1) NOT NULL;

DECLARE @PrimaryKeyName sysname;
DECLARE @HasMaCTPPPrimaryKey bit = 0;
DECLARE @DropPrimaryKeySql nvarchar(max);

SELECT @PrimaryKeyName = kc.name
FROM sys.key_constraints kc
WHERE kc.parent_object_id = OBJECT_ID('dbo.ChiTietPhieuPhat')
  AND kc.type = 'PK';

IF @PrimaryKeyName IS NOT NULL
   AND EXISTS (
        SELECT 1
        FROM sys.key_constraints kc
        JOIN sys.index_columns ic
          ON ic.object_id = kc.parent_object_id
         AND ic.index_id = kc.unique_index_id
        JOIN sys.columns c
          ON c.object_id = ic.object_id
         AND c.column_id = ic.column_id
        WHERE kc.parent_object_id = OBJECT_ID('dbo.ChiTietPhieuPhat')
          AND kc.type = 'PK'
          AND c.name = 'MaCTPP'
          AND ic.key_ordinal = 1
          AND (SELECT COUNT(*)
               FROM sys.index_columns allColumns
               WHERE allColumns.object_id = ic.object_id
                 AND allColumns.index_id = ic.index_id
                 AND allColumns.key_ordinal > 0) = 1
   )
    SET @HasMaCTPPPrimaryKey = 1;

IF @PrimaryKeyName IS NOT NULL AND @HasMaCTPPPrimaryKey = 0
BEGIN
    SET @DropPrimaryKeySql = N'ALTER TABLE dbo.ChiTietPhieuPhat DROP CONSTRAINT ' + QUOTENAME(@PrimaryKeyName);
    EXEC sys.sp_executesql @DropPrimaryKeySql;
END

IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.ChiTietPhieuPhat')
      AND name = 'MaSach'
      AND is_nullable = 1
)
    ALTER TABLE dbo.ChiTietPhieuPhat ALTER COLUMN MaSach VARCHAR(20) NOT NULL;

IF @HasMaCTPPPrimaryKey = 0
    ALTER TABLE dbo.ChiTietPhieuPhat
    ADD CONSTRAINT PK_ChiTietPhieuPhat PRIMARY KEY (MaCTPP);

IF NOT EXISTS (
    SELECT 1 FROM sys.indexes
    WHERE object_id = OBJECT_ID('dbo.ChiTietPhieuPhat')
      AND name = 'UX_ChiTietPhieuPhat_Phieu_Sach_QuyDinh'
)
    CREATE UNIQUE INDEX UX_ChiTietPhieuPhat_Phieu_Sach_QuyDinh
    ON dbo.ChiTietPhieuPhat (MaPhieuPhat, MaSach, MaQD);

IF EXISTS (
    SELECT 1 FROM sys.columns
    WHERE object_id = OBJECT_ID('dbo.PhieuPhat')
      AND name = 'MaPhieuMuon'
      AND is_nullable = 1
)
    ALTER TABLE dbo.PhieuPhat ALTER COLUMN MaPhieuMuon VARCHAR(20) NOT NULL;

COMMIT TRANSACTION;
GO
