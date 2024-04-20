# Employee Management System

###  Stored Procedure

```
CREATE procedure [dbo].[sp_Employees]  
@Pageindex int,  
@Pagesize int,  
@SearchName varchar(50),
@Arraystring varchar(100)  
as  
begin  

IF @Arraystring = ''
    BEGIN
	-- Declares a variable @CSVString to store the comma-separated department IDs.
        DECLARE @CSVString VARCHAR(MAX) = '';
		-- Constructs the CSV string by concatenating department IDs.
        SELECT @CSVString = COALESCE(@CSVString + ',', '') + CONVERT(VARCHAR(10), DepartmentId)
        FROM (  
            SELECT DISTINCT DepartmentId
            FROM Employees
        ) AS subquery

        SET @Arraystring = @CSVString; -- Set the value to @Arraystring
    END

 select * from Employees Where Name LIKE @SearchName+'%'  
 AND DepartmentId IN(Select value from string_split(@Arraystring,','))  
 order by Id ASC Offset @Pagesize*(@Pageindex-1) Rows   
 fetch next @Pagesize Rows only  
end
```
