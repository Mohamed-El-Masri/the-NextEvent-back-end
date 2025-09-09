using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheNextEventAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSectionKeyToWebsiteContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SeoMetadata_PageKey",
                table: "SeoMetadata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebsiteContent",
                table: "WebsiteContent");

            migrationBuilder.DropIndex(
                name: "UK_WebsiteContent_Section_Key",
                table: "WebsiteContent");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailConfiguration",
                table: "EmailConfiguration");

            migrationBuilder.DropIndex(
                name: "IX_EmailConfiguration_ConfigKey",
                table: "EmailConfiguration");

            migrationBuilder.DropColumn(
                name: "PageKey",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "EmailNotificationSent",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "EmailNotificationStatus",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "FormType",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "IPAddress",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "SubmissionData",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "AdminUsers");

            migrationBuilder.DropColumn(
                name: "ConfigKey",
                table: "EmailConfiguration");

            migrationBuilder.DropColumn(
                name: "ConfigValue",
                table: "EmailConfiguration");

            migrationBuilder.DropColumn(
                name: "IsEncrypted",
                table: "EmailConfiguration");

            migrationBuilder.RenameTable(
                name: "WebsiteContent",
                newName: "WebsiteContents");

            migrationBuilder.RenameTable(
                name: "EmailConfiguration",
                newName: "EmailConfigurations");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "SeoMetadata",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "SubmissionDate",
                table: "FormSubmissions",
                newName: "SubmittedAt");

            migrationBuilder.RenameColumn(
                name: "LastLogin",
                table: "AdminUsers",
                newName: "LastLoginAt");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "WebsiteContents",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "LastUpdated",
                table: "EmailConfigurations",
                newName: "UpdatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "TitleAR",
                table: "SeoMetadata",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SeoMetadata",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "KeywordsAR",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Keywords",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAR",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CanonicalUrl",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "SeoMetadata",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "SeoMetadata",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "OgDescription",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OgDescriptionAR",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OgImage",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OgTitle",
                table: "SeoMetadata",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OgTitleAR",
                table: "SeoMetadata",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PageUrl",
                table: "SeoMetadata",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FormSubmissions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "New",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "AdminNotes",
                table: "FormSubmissions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Company",
                table: "FormSubmissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "FormSubmissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "FormSubmissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "FormSubmissions",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FormSubmissions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "FormSubmissions",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AdminUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AdminUsers",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AdminUsers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SectionKey",
                table: "WebsiteContents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NameAR",
                table: "WebsiteContents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WebsiteContents",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "WebsiteContents",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "WebsiteContents",
                type: "bit",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAR",
                table: "WebsiteContents",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WebsiteContents",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContentKey",
                table: "WebsiteContents",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "WebsiteContents",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EmailConfigurations",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "EmailConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "NotificationEmails",
                table: "EmailConfigurations",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SenderEmail",
                table: "EmailConfigurations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderName",
                table: "EmailConfigurations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderPassword",
                table: "EmailConfigurations",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SmtpPort",
                table: "EmailConfigurations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SmtpServer",
                table: "EmailConfigurations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "UseSSL",
                table: "EmailConfigurations",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebsiteContents",
                table: "WebsiteContents",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailConfigurations",
                table: "EmailConfigurations",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SeoMetadata_PageUrl",
                table: "SeoMetadata",
                column: "PageUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_WebsiteContent_ContentKey",
                table: "WebsiteContents",
                column: "ContentKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SeoMetadata_PageUrl",
                table: "SeoMetadata");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WebsiteContents",
                table: "WebsiteContents");

            migrationBuilder.DropIndex(
                name: "UK_WebsiteContent_ContentKey",
                table: "WebsiteContents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EmailConfigurations",
                table: "EmailConfigurations");

            migrationBuilder.DropColumn(
                name: "CanonicalUrl",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "OgDescription",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "OgDescriptionAR",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "OgImage",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "OgTitle",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "OgTitleAR",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "PageUrl",
                table: "SeoMetadata");

            migrationBuilder.DropColumn(
                name: "AdminNotes",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "Company",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "FormSubmissions");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AdminUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AdminUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AdminUsers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "WebsiteContents");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EmailConfigurations");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "EmailConfigurations");

            migrationBuilder.DropColumn(
                name: "NotificationEmails",
                table: "EmailConfigurations");

            migrationBuilder.DropColumn(
                name: "SenderEmail",
                table: "EmailConfigurations");

            migrationBuilder.DropColumn(
                name: "SenderName",
                table: "EmailConfigurations");

            migrationBuilder.DropColumn(
                name: "SenderPassword",
                table: "EmailConfigurations");

            migrationBuilder.DropColumn(
                name: "SmtpPort",
                table: "EmailConfigurations");

            migrationBuilder.DropColumn(
                name: "SmtpServer",
                table: "EmailConfigurations");

            migrationBuilder.DropColumn(
                name: "UseSSL",
                table: "EmailConfigurations");

            migrationBuilder.RenameTable(
                name: "WebsiteContents",
                newName: "WebsiteContent");

            migrationBuilder.RenameTable(
                name: "EmailConfigurations",
                newName: "EmailConfiguration");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "SeoMetadata",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "SubmittedAt",
                table: "FormSubmissions",
                newName: "SubmissionDate");

            migrationBuilder.RenameColumn(
                name: "LastLoginAt",
                table: "AdminUsers",
                newName: "LastLogin");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "WebsiteContent",
                newName: "LastUpdated");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "EmailConfiguration",
                newName: "LastUpdated");

            migrationBuilder.AlterColumn<string>(
                name: "TitleAR",
                table: "SeoMetadata",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "SeoMetadata",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "KeywordsAR",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Keywords",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAR",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SeoMetadata",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AddColumn<string>(
                name: "PageKey",
                table: "SeoMetadata",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "FormSubmissions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "New");

            migrationBuilder.AddColumn<bool>(
                name: "EmailNotificationSent",
                table: "FormSubmissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmailNotificationStatus",
                table: "FormSubmissions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormType",
                table: "FormSubmissions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IPAddress",
                table: "FormSubmissions",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubmissionData",
                table: "FormSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "AdminUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SectionKey",
                table: "WebsiteContent",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "NameAR",
                table: "WebsiteContent",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "WebsiteContent",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "WebsiteContent",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "WebsiteContent",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionAR",
                table: "WebsiteContent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "WebsiteContent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.AlterColumn<string>(
                name: "ContentKey",
                table: "WebsiteContent",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ConfigKey",
                table: "EmailConfiguration",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConfigValue",
                table: "EmailConfiguration",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsEncrypted",
                table: "EmailConfiguration",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WebsiteContent",
                table: "WebsiteContent",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EmailConfiguration",
                table: "EmailConfiguration",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_SeoMetadata_PageKey",
                table: "SeoMetadata",
                column: "PageKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UK_WebsiteContent_Section_Key",
                table: "WebsiteContent",
                columns: new[] { "SectionKey", "ContentKey" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfiguration_ConfigKey",
                table: "EmailConfiguration",
                column: "ConfigKey",
                unique: true);
        }
    }
}
