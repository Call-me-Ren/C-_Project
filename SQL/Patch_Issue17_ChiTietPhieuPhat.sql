SET NOCOUNT ON;
SET XACT_ABORT ON;

BEGIN TRY
    BEGIN TRANSACTION;
    DECLARE @Sql NVARCHAR(1000);

    IF OBJECT_ID(N'dbo.ChiTietPhieuPhat', N'U') IS NULL
    BEGIN
        RAISERROR(N'Không tìm thấy bảng ChiTietPhieuPhat.', 16, 1);
    END;

    IF EXISTS (SELECT 1 FROM dbo.ChiTietPhieuPhat WHERE MaSach IS NULL)
    BEGIN
        RAISERROR(N'Có vi phạm chưa gắn với cuốn sách.', 16, 1);
    END;

    IF COL_LENGTH(N'dbo.ChiTietPhieuPhat', N'MaCTPP') IS NULL
        ALTER TABLE dbo.ChiTietPhieuPhat ADD MaCTPP INT IDENTITY(1,1) NOT NULL;
    ELSE IF NOT EXISTS (
        SELECT 1
        FROM sys.columns
        WHERE object_id = OBJECT_ID(N'dbo.ChiTietPhieuPhat')
          AND name = N'MaCTPP'
          AND system_type_id = TYPE_ID(N'int')
          AND is_identity = 1
    )
    BEGIN
        RAISERROR(N'Cột MaCTPP cần là số nguyên tự tăng.', 16, 1);
    END;

    IF EXISTS (
        SELECT 1 FROM sys.columns
        WHERE object_id = OBJECT_ID(N'dbo.ChiTietPhieuPhat')
          AND name = N'MaSach' AND is_nullable = 1
    )
        ALTER TABLE dbo.ChiTietPhieuPhat ALTER COLUMN MaSach VARCHAR(20) NOT NULL;

    DECLARE @PrimaryKeyName SYSNAME;
    SELECT @PrimaryKeyName = name
    FROM sys.key_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.ChiTietPhieuPhat') AND type = N'PK';

    IF @PrimaryKeyName IS NOT NULL AND NOT EXISTS (
        SELECT 1
        FROM sys.key_constraints kc
        WHERE kc.parent_object_id = OBJECT_ID(N'dbo.ChiTietPhieuPhat')
          AND kc.name = @PrimaryKeyName
          AND (SELECT COUNT(1) FROM sys.index_columns ic
               WHERE ic.object_id = kc.parent_object_id
                 AND ic.index_id = kc.unique_index_id AND ic.key_ordinal > 0) = 1
          AND EXISTS (
              SELECT 1 FROM sys.index_columns ic
              JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
              WHERE ic.object_id = kc.parent_object_id AND ic.index_id = kc.unique_index_id
                AND ic.key_ordinal = 1 AND c.name = N'MaCTPP'
          )
    )
    BEGIN
        SET @Sql = N'ALTER TABLE dbo.ChiTietPhieuPhat DROP CONSTRAINT ' + QUOTENAME(@PrimaryKeyName);
        EXEC sp_executesql @Sql;
        SET @PrimaryKeyName = NULL;
    END;

    IF @PrimaryKeyName IS NULL
        ALTER TABLE dbo.ChiTietPhieuPhat
            ADD CONSTRAINT PK_ChiTietPhieuPhat PRIMARY KEY (MaCTPP);

    DECLARE @OldUniqueName SYSNAME;
    DECLARE @OldUniqueIsConstraint BIT;
    WHILE EXISTS (
        SELECT 1
        FROM sys.indexes i
        WHERE i.object_id = OBJECT_ID(N'dbo.ChiTietPhieuPhat')
          AND i.is_unique = 1 AND i.is_primary_key = 0
          AND (SELECT COUNT(1) FROM sys.index_columns ic
               WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.key_ordinal > 0) = 2
          AND EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c
                      ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                      WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = N'MaPhieuPhat')
          AND EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c
                      ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                      WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = N'MaQD')
    )
    BEGIN
        SELECT TOP (1)
            @OldUniqueName = i.name,
            @OldUniqueIsConstraint = CASE WHEN kc.name IS NULL THEN 0 ELSE 1 END
        FROM sys.indexes i
        LEFT JOIN sys.key_constraints kc
          ON kc.parent_object_id = i.object_id AND kc.unique_index_id = i.index_id AND kc.type = N'UQ'
        WHERE i.object_id = OBJECT_ID(N'dbo.ChiTietPhieuPhat')
          AND i.is_unique = 1 AND i.is_primary_key = 0
          AND (SELECT COUNT(1) FROM sys.index_columns ic
               WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.key_ordinal > 0) = 2
          AND EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c
                      ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                      WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = N'MaPhieuPhat')
          AND EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c
                      ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                      WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = N'MaQD');

        IF @OldUniqueIsConstraint = 1
            SET @Sql = N'ALTER TABLE dbo.ChiTietPhieuPhat DROP CONSTRAINT ' + QUOTENAME(@OldUniqueName);
        ELSE
            SET @Sql = N'DROP INDEX ' + QUOTENAME(@OldUniqueName) + N' ON dbo.ChiTietPhieuPhat';
        EXEC sp_executesql @Sql;
    END;

    IF NOT EXISTS (
        SELECT 1
        FROM sys.indexes i
        WHERE i.object_id = OBJECT_ID(N'dbo.ChiTietPhieuPhat')
          AND i.is_unique = 1 AND i.has_filter = 0
          AND (SELECT COUNT(1) FROM sys.index_columns ic
               WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.key_ordinal > 0) = 3
          AND EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c
                      ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                      WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = N'MaPhieuPhat')
          AND EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c
                      ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                      WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = N'MaSach')
          AND EXISTS (SELECT 1 FROM sys.index_columns ic JOIN sys.columns c
                      ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                      WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = N'MaQD')
    )
    BEGIN
        IF EXISTS (
            SELECT 1 FROM dbo.ChiTietPhieuPhat
            GROUP BY MaPhieuPhat, MaSach, MaQD HAVING COUNT(1) > 1
        )
        BEGIN
            RAISERROR(N'Có vi phạm trùng cuốn sách và quy định trong cùng phiếu.', 16, 1);
        END;

        ALTER TABLE dbo.ChiTietPhieuPhat
            ADD CONSTRAINT UQ_ChiTietPhieuPhat_Phieu_Sach_QuyDinh UNIQUE (MaPhieuPhat, MaSach, MaQD);
    END;

    COMMIT TRANSACTION;
    PRINT N'Bảng ChiTietPhieuPhat đã đúng cấu trúc.';
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
    THROW;
END CATCH;
