﻿namespace PopStudio.Language.Languages
{
    internal class MAUIENUS : ILocalization
    {
        //Article
        public string Article_Title => "Article";

        //Command
        public string Command_EnterInteger => "Please enter an integer...";
        public string Command_EnterString => "Please enter a string...";
        public string Command_EnterFormat => "Please enter order number of the format";
        //Atlas
        public string Atlas_Title => "Atlas";
        public string Atlas_Introduction => "Through the atlas function, build small pictures into atlas or cut the atlas into small pictures! ";
        public string Atlas_Mode1 => "Cut Atlas";
        public string Atlas_Mode2 => "Build Atlas";
        public string Atlas_Choose1 => "Path of cut atlas";
        public string Atlas_Choose2 => "Path of out folder";
        public string Atlas_Choose3 => "Path of cut info";
        public string Atlas_Choose4 => "ID of cut atlas(omissible)";
        public string Atlas_Choose5 => "Path of built folder";
        public string Atlas_Choose6 => "Path of out atlas";
        public string Atlas_Choose7 => "ID of building atlas(omissible)";
        public string Atlas_Format => "Format of cut info";
        public string Atlas_MaxWidth => "Width of out atlas";
        public string Atlas_MaxHeight => "Height of out atlas";
        public string Atlas_NotFound1 => "Can not find cutting information";
        public string Atlas_NotFound2 => "Atlas generation succeeded, but no cutting information was found";

        //LuaScript
        public string LuaScript_Title => "Lua Script";
        public string LuaScript_Introduction => "Through lua script, call the built-in function of the program to realize batch processing and other functions! ";
        public string LuaScript_TracePrint => "Print";

        //Shell
        public string Shell_OK => "Yes\0";
        public string Shell_Cancel => "No\0";
        public string Shell_ExitTitle => "Exit";
        public string Shell_ExitText => "Are you sure you want to exit?";

        //Setting
        public string Setting_Title => "Setting";
        public string Setting_Introduction => "Change the default behavior of the tool by setting!";
        public string Setting_ItemLanguage => "Language: {0}";
        public string Setting_SetLanguage => "Change Language";
        public string Setting_Extension => "Extension";
        public string Setting_CompressionMethod => "Compression Method";
        public string Setting_ItemDz => "Compression Method of Dz Packing";
        public string Setting_IntroDz => "Different compression methods can be selected for each format of files when packing Dz. Store, Lzma, Gzip and Bzip2 compression are supported now. If the compression method of a format is not specified, the default compression method will be adopted. ";
        public string Setting_Add => "Add New";
        public string Setting_Clear => "Clear All";
        public string Setting_ItemPak => "Compression Method of PakPS3 Packing";
        public string Setting_IntroPak => "Different compression methods can be selected for each format of files when packing Pak for PS3. Store and Zlib compression are supported now. If the compression method of a format is not specified, the default compression method will be adopted. ";
        public string Setting_ItemRsb => "PTX0 Format of Rsb Unpacking and Packing";
        public string Setting_IntroRsb => "If it is necessary to decode and encode PTX when unpacking and packing Rsb, you need to specify the decoding and encoding method of ptx0. If neither is checked, it will use ARGB8888 mode. ";
        public string Setting_ItemPtx => "PTX0 Format for decoding";
        public string Setting_IntroPtx => "When decoding PTX0, you need to specify the decoding method. If neither is checked, it will use ARGB8888 mode. ";
        public string Setting_ItemCdat => "Cdat Cipher";
        public string Setting_IntroCdat => "Cdat is the encryption PNG format of PVZ free. The program provides the key by default, and you can change it by yourself. ";
        public string Setting_ItemRTON => "RTON Cipher";
        public string Setting_IntroRTON => "Some RTON is in encryption format. The tool does not provide a key. You need to provide your own key to decrypt and encrypt.";
        public string Setting_ItemCompiled => "Convert Number Image Label";
        public string Setting_IntroCompiled => "In some platform's compiled files in PVZ, the image label is changed from string to number. You can find the corresponding relationship in so and provide it to the tool to change the number into string.";
        public string Setting_ItemXfl => "Xfl Generated Setting";
        public string Setting_IntroXfl => "When using reanim transcoding to generate Flash_Xfl document to use Adobe Animate to edit it, you can specify the canvas size, whether the generated picture name uses the I label signature or the name label name, and the scaling multiple of the horizontal and vertical coordinates. ";
        public string Setting_XflWidth => "Canva Width";
        public string Setting_XflHeight => "Canva Height";
        public string Setting_XflLabelName => "Use Label Name";
        public string Setting_XflScaleX => "Scale X";
        public string Setting_XflScaleY => "Scale Y";
        public string Setting_AD => "Load ads when program starts";
        public string Setting_Load => "Load";
        public string Setting_Unload => "Unload";
        public string Setting_Recover => "Restore Default Settings";
        public string Setting_OK => "OK\0";
        public string Setting_Cancel => "Cancel\0";
        public string Setting_SureRecover => "Are you sure? ";
        public string Setting_SureRecoverText => "Are you sure you want to restore the default settings? This operation can not be reversible! ";
        public string Setting_FinishRecover => "Recovery Complete";
        public string Setting_FinishRecoverText => "Restore default settings complete. The tool needs to be restarted immediately. ";
        public string Setting_Submit => "Submit";
        public string Setting_ChooseItem => "Please Select One";
        public string Setting_ChooseCompressMode => "Please Select Method";
        public string Setting_EnterExtension => "Please Write the File Extension";
        public string Setting_EnterExtensionText => "Please Write the File Extension. Files with the same extension will be compressed in the same way. ";
        public string Setting_CompressItem1 => "Change Compression Method";
        public string Setting_CompressItem2 => "Delete this Item";
        public string Setting_ChooseLanguage => "Please Choose Language";
        public string Setting_FinishChooseLanguage => "Language Setting Complete";
        public string Setting_FinishChooseLanguageText => "Language setting complete. The tool needs to be restarted immediately. ";

        //Texture
        public string Texture_Title => "Image";
        public string Texture_Introduction => "Through image transcoding, obtain and modify the special image content of the game! ";
        public string Texture_Mode1 => "Decode";
        public string Texture_Mode2 => "Encode";
        public string Texture_Choose1 => "The decoded special image path";
        public string Texture_Choose2 => "The path of generated PNG image";
        public string Texture_Choose3 => "Decoding Mode";
        public string Texture_Choose4 => "The encoded PNG image path";
        public string Texture_Choose5 => "The path of generated special image";
        public string Texture_Choose6 => "Encoding Mode";
        public string Texture_Choose7 => "Encoding Format";

        //Pam
        public string Pam_Title => "Pam";
        public string Pam_Introduction => "Through PAM transcoding, you can convert PAM animation file and json file to modify game animation! Pam versions 1 to 6 are all supported! You can also use Twinkles-Toolkit to convert the generated JSON file into XFL animation for Adobe animation to view! ";
        public string Pam_Mode1 => "Decode";
        public string Pam_Mode2 => "Encode";
        public string Pam_Choose1 => "The decoded file path";
        public string Pam_Choose2 => "The path of generated file";
        public string Pam_Choose3 => "Decoding Mode";
        public string Pam_Choose4 => "The encoded file path";
        public string Pam_Choose5 => "The path of generated file";
        public string Pam_Choose6 => "Encoding Mode";

        //RTON
        public string RTON_Title => "RTON";
        public string RTON_Introduction => "Through RTON transcoding, you can convert RTON file and json file to modify the content of games! If you choose the encrypted rton mode, you need to fill in the key in the setting. ";
        public string RTON_Mode1 => "Decode";
        public string RTON_Mode2 => "Encode";
        public string RTON_Choose1 => "The decoded file path";
        public string RTON_Choose2 => "The path of generated file";
        public string RTON_Choose3 => "Decoding Mode";
        public string RTON_Choose4 => "The encoded file path";
        public string RTON_Choose5 => "The path of generated file";
        public string RTON_Choose6 => "Encoding Mode";

        //Trail
        public string Trail_Title => "Trail";
        public string Trail_Introduction => "Through trail transcoding, you can obtain and modify the content of the game trail effect! ";
        public string Trail_Choose1 => "Path of in file";
        public string Trail_Choose2 => "Path of out file";
        public string Trail_InFormat => "Format of in file";
        public string Trail_OutFormat => "Format of out file";

        //Reanim
        public string Reanim_Title => "Reanim";
        public string Reanim_Introduction => "Obtain and modify the game action content through reanim transcoding! ";
        public string Reanim_Choose1 => "Path of in file";
        public string Reanim_Choose2 => "Path of out file";
        public string Reanim_InFormat => "Format of in file";
        public string Reanim_OutFormat => "Format of out file";

        //Particles
        public string Particles_Title => "Particles";
        public string Particles_Introduction => "Get and modify the content of game particle effects through particles transcoding! ";
        public string Particles_Choose1 => "Path of in file";
        public string Particles_Choose2 => "Path of out file";
        public string Particles_InFormat => "Format of in file";
        public string Particles_OutFormat => "Format of out file";

        //Compress
        public string Compress_Title => "Compress";
        public string Compress_Introduction => "Through compress, you can decompress and compress a single file. ";
        public string Compress_Mode1 => "Decompress";
        public string Compress_Mode2 => "Compress";
        public string Compress_Choose1 => "The decompressed file path";
        public string Compress_Choose2 => "The path of generated file";
        public string Compress_Choose3 => "Decompressing Mode";
        public string Compress_Choose4 => "The compressed file path";
        public string Compress_Choose5 => "The path of generated file";
        public string Compress_Choose6 => "Compressing Mode";

        //Package
        public string Package_Title => "Package";
        public string Package_Introduction => "Get game materials and modify game images through package! ";
        public string Package_Mode1 => "Unpack";
        public string Package_Mode2 => "Pack";
        public string Package_Choose1 => "The unpacked file path";
        public string Package_Choose2 => "The path of generated folder";
        public string Package_Choose3 => "Unpacking Mode";
        public string Package_Choose4 => "The packed folder path";
        public string Package_Choose5 => "The path of generated file";
        public string Package_Choose6 => "Packing Mode";
        public string Package_ChangeImage => "Convert all special images to png. ";
        public string Package_DeleteImage => "Delete the special image after being converted to png. ";

        //HomePage
        public string HomePage_Title => "HomePage";
        public string HomePage_Begin => "Start your creative journey!";
        public string HomePage_Function => "Modify the data package, images, actions, particle effects and other contents of PopCap Games by using PopStudio! ";
        public string HomePage_Permission => "On Android 6 and above systems, the program must have storage permission to operate normally. Click the button to give permission:";
        public string HomePage_PermissionAsk => "Give Permission";
        public string HomePage_Agreement => "If you use this tool to make works, please be sure to indicate the use of this tool in an eye-catching position. Commercial use of this tool must be approved by the author, otherwise it will be deemed as infringement! ";
        public string HomePage_Version => "PopStudio Version {0}";
        public string HomePage_Author_String => "Author:";
        public string HomePage_Author => "YingFengTingYu";
        public string HomePage_Thanks_String => "Thanks:";
        public string HomePage_Thanks => "2508, Japonisme Toumai, 补补23456, 孤独泪痕, 63enjoy, AS魇梦蚀, 伊特, 某个萌新, 天天, Indestructible_Ch, 僵学者, An-Haze";
        public string HomePage_QQGroup_String => "QQ group:";
        public string HomePage_QQGroup => "1017246977";
        public string HomePage_Course_String => "Course Video:";
        public string HomePage_Course => "https://space.bilibili.com/411256864";
        public string HomePage_AppNewNotice_String => "Update announcement:";
        public string HomePage_AppNewNotice => "1. Support PAM decoding and encoding; \n2. Repair script function \"rainy.dofile\"; \n3. Support calling program by CMD in Windows, MacOS and Linux system.";
        //Share
        public string Share_FileNotFound => "File {0} does not exist! ";
        public string Share_FolderNotFound => "Folder {0} does not exist! ";
        public string Share_Finish => "Executing complete with {0} seconds";
        public string Share_Finish_NoTime => "Executing complete";
        public string Share_Wrong => "Executing error: {0}";
        public string Share_ChooseMode => "Select Operation Mode";
        public string Share_Choose => "Select";
        public string Share_Run => "Execution";
        public string Share_RunStatue => "Execution status:";
        public string Share_Waiting => "Waiting";
        public string Share_Running => "Executing......";

        //Permission
        public string Permission_Title => "Permissions Request";
        public string Permission_Request1 => "In Android 6 and above system versions, please grant the read and write storage permission, otherwise the program will not have the right to read and write files! ";
        public string Permission_Request2 => "In Android 11 and above system versions, please grant the manage storage permission, otherwise the program will only read and write files in the internal folder of the program! ";
        public string Permission_RequestFinish => "Already has storage permission! ";
        public string Permission_GoTo => "Authorization";
        public string Permission_OK => "OK";
        public string Permission_Cancel => "Cancel";

        //PickFile
        public string PickFile_AllFiles => "All Files";
        public string PickFile_NewFolder => "New Folder\0";
        public string PickFile_Back => "↩️Return to parent directory\0";
        public string PickFile_OK => "OK\0";
        public string PickFile_Cancel => "Cancel\0";
        public string PickFile_EnterFolderName => "Please enter a folder name";
        public string PickFile_CreateWrong => "Creating error";
        public string PickFile_NoPermission => "No permission";
        public string PickFile_NoPermissionToEnter => "Failed to enter the folder. The program has no access rights. ";
        public string PickFlie_SaveThere => "Save there\0";
        public string PickFlie_SaveFile => "Save file";
        public string PickFile_EnterFileName => "Please enter a file name";
        public string PickFile_ChooseThisFolder => "Select folder\0";

        //AD
        public string AD_Cancel => "Cancel";
        public string AD_Title => "Advertisement";
    }
}
