; ModuleID = 'marshal_methods.arm64-v8a.ll'
source_filename = "marshal_methods.arm64-v8a.ll"
target datalayout = "e-m:e-i8:8:32-i16:16:32-i64:64-i128:128-n32:64-S128"
target triple = "aarch64-unknown-linux-android21"

%struct.MarshalMethodName = type {
	i64, ; uint64_t id
	ptr ; char* name
}

%struct.MarshalMethodsManagedClass = type {
	i32, ; uint32_t token
	ptr ; MonoClass klass
}

@assembly_image_cache = dso_local local_unnamed_addr global [184 x ptr] zeroinitializer, align 8

; Each entry maps hash of an assembly name to an index into the `assembly_image_cache` array
@assembly_image_cache_hashes = dso_local local_unnamed_addr constant [552 x i64] [
	i64 u0x0071cf2d27b7d61e, ; 0: lib_Xamarin.AndroidX.SwipeRefreshLayout.dll.so => 101
	i64 u0x0075ea86498d3e4d, ; 1: OpenAI => 72
	i64 u0x01109b0e4d99e61f, ; 2: System.ComponentModel.Annotations.dll => 114
	i64 u0x020f428300334897, ; 3: Grpc.Net.Client.dll => 51
	i64 u0x02123411c4e01926, ; 4: lib_Xamarin.AndroidX.Navigation.Runtime.dll.so => 97
	i64 u0x02abedc11addc1ed, ; 5: lib_Mono.Android.Runtime.dll.so => 182
	i64 u0x032267b2a94db371, ; 6: lib_Xamarin.AndroidX.AppCompat.dll.so => 80
	i64 u0x038844948b6e3251, ; 7: AI_Translator_Mobile_App.dll => 108
	i64 u0x043032f1d071fae0, ; 8: ru/Microsoft.Maui.Controls.resources => 24
	i64 u0x044440a55165631e, ; 9: lib-cs-Microsoft.Maui.Controls.resources.dll.so => 2
	i64 u0x046eb1581a80c6b0, ; 10: vi/Microsoft.Maui.Controls.resources => 30
	i64 u0x0517ef04e06e9f76, ; 11: System.Net.Primitives => 139
	i64 u0x0565d18c6da3de38, ; 12: Xamarin.AndroidX.RecyclerView => 99
	i64 u0x0581db89237110e9, ; 13: lib_System.Collections.dll.so => 113
	i64 u0x05989cb940b225a9, ; 14: Microsoft.Maui.dll => 67
	i64 u0x06076b5d2b581f08, ; 15: zh-HK/Microsoft.Maui.Controls.resources => 31
	i64 u0x06388ffe9f6c161a, ; 16: System.Xml.Linq.dll => 175
	i64 u0x0680a433c781bb3d, ; 17: Xamarin.AndroidX.Collection.Jvm => 83
	i64 u0x07c57877c7ba78ad, ; 18: ru/Microsoft.Maui.Controls.resources.dll => 24
	i64 u0x07dcdc7460a0c5e4, ; 19: System.Collections.NonGeneric => 111
	i64 u0x08a7c865576bbde7, ; 20: System.Reflection.Primitives => 153
	i64 u0x08f3c9788ee2153c, ; 21: Xamarin.AndroidX.DrawerLayout => 88
	i64 u0x09138715c92dba90, ; 22: lib_System.ComponentModel.Annotations.dll.so => 114
	i64 u0x0919c28b89381a0b, ; 23: lib_Microsoft.Extensions.Options.dll.so => 63
	i64 u0x092266563089ae3e, ; 24: lib_System.Collections.NonGeneric.dll.so => 111
	i64 u0x095cacaf6b6a32e4, ; 25: System.Memory.Data => 78
	i64 u0x09d144a7e214d457, ; 26: System.Security.Cryptography => 165
	i64 u0x0abb3e2b271edc45, ; 27: System.Threading.Channels.dll => 170
	i64 u0x0b3b632c3bbee20c, ; 28: sk/Microsoft.Maui.Controls.resources => 25
	i64 u0x0b6aff547b84fbe9, ; 29: Xamarin.KotlinX.Serialization.Core.Jvm => 107
	i64 u0x0be2e1f8ce4064ed, ; 30: Xamarin.AndroidX.ViewPager => 102
	i64 u0x0c279376b1ae96ae, ; 31: lib_System.CodeDom.dll.so => 76
	i64 u0x0c3ca6cc978e2aae, ; 32: pt-BR/Microsoft.Maui.Controls.resources => 21
	i64 u0x0c59ad9fbbd43abe, ; 33: Mono.Android => 183
	i64 u0x0c7790f60165fc06, ; 34: lib_Microsoft.Maui.Essentials.dll.so => 68
	i64 u0x0d3b5ab8b2766190, ; 35: lib_Microsoft.Bcl.AsyncInterfaces.dll.so => 53
	i64 u0x0d565cb22b8879da, ; 36: lib_Grpc.Core.Api.dll.so => 50
	i64 u0x0e14e73a54dda68e, ; 37: lib_System.Net.NameResolution.dll.so => 137
	i64 u0x0eb9a03556d6e554, ; 38: GenerativeAI => 48
	i64 u0x0f5e7abaa7cf470a, ; 39: System.Net.HttpListener => 136
	i64 u0x102861e4055f511a, ; 40: Microsoft.Bcl.AsyncInterfaces.dll => 53
	i64 u0x102a31b45304b1da, ; 41: Xamarin.AndroidX.CustomView => 87
	i64 u0x10f6cfcbcf801616, ; 42: System.IO.Compression.Brotli => 126
	i64 u0x114443cdcf2091f1, ; 43: System.Security.Cryptography.Primitives => 163
	i64 u0x11a70d0e1009fb11, ; 44: System.Net.WebSockets.dll => 145
	i64 u0x123639456fb056da, ; 45: System.Reflection.Emit.Lightweight.dll => 152
	i64 u0x125b7f94acb989db, ; 46: Xamarin.AndroidX.RecyclerView.dll => 99
	i64 u0x13a01de0cbc3f06c, ; 47: lib-fr-Microsoft.Maui.Controls.resources.dll.so => 8
	i64 u0x13f1e5e209e91af4, ; 48: lib_Java.Interop.dll.so => 181
	i64 u0x13f1e880c25d96d1, ; 49: he/Microsoft.Maui.Controls.resources => 9
	i64 u0x143d8ea60a6a4011, ; 50: Microsoft.Extensions.DependencyInjection.Abstractions => 58
	i64 u0x1497051b917530bd, ; 51: lib_System.Net.WebSockets.dll.so => 145
	i64 u0x15bdc156ed462f2f, ; 52: lib_System.IO.FileSystem.dll.so => 129
	i64 u0x16bf2a22df043a09, ; 53: System.IO.Pipes.dll => 131
	i64 u0x16ea2b318ad2d830, ; 54: System.Security.Cryptography.Algorithms => 162
	i64 u0x17125c9a85b4929f, ; 55: lib_netstandard.dll.so => 179
	i64 u0x17b56e25558a5d36, ; 56: lib-hu-Microsoft.Maui.Controls.resources.dll.so => 12
	i64 u0x17f9358913beb16a, ; 57: System.Text.Encodings.Web => 167
	i64 u0x18402a709e357f3b, ; 58: lib_Xamarin.KotlinX.Serialization.Core.Jvm.dll.so => 107
	i64 u0x18f0ce884e87d89a, ; 59: nb/Microsoft.Maui.Controls.resources.dll => 18
	i64 u0x19a4c090f14ebb66, ; 60: System.Security.Claims => 161
	i64 u0x19ee7b9db439f552, ; 61: Anthropic.SDK.dll => 35
	i64 u0x1a91866a319e9259, ; 62: lib_System.Collections.Concurrent.dll.so => 110
	i64 u0x1aac34d1917ba5d3, ; 63: lib_System.dll.so => 178
	i64 u0x1aad60783ffa3e5b, ; 64: lib-th-Microsoft.Maui.Controls.resources.dll.so => 27
	i64 u0x1c753b5ff15bce1b, ; 65: Mono.Android.Runtime.dll => 182
	i64 u0x1cb6a0ededc839e2, ; 66: lib_Google.Apis.Auth.dll.so => 41
	i64 u0x1dba6509cc55b56f, ; 67: lib_Google.Protobuf.dll.so => 47
	i64 u0x1e3d87657e9659bc, ; 68: Xamarin.AndroidX.Navigation.UI => 98
	i64 u0x1e71143913d56c10, ; 69: lib-ko-Microsoft.Maui.Controls.resources.dll.so => 16
	i64 u0x1ed8fcce5e9b50a0, ; 70: Microsoft.Extensions.Options.dll => 63
	i64 u0x209375905fcc1bad, ; 71: lib_System.IO.Compression.Brotli.dll.so => 126
	i64 u0x20e085517023eec8, ; 72: lib_Google.Api.Gax.dll.so => 38
	i64 u0x20fab3cf2dfbc8df, ; 73: lib_System.Diagnostics.Process.dll.so => 121
	i64 u0x2174319c0d835bc9, ; 74: System.Runtime => 160
	i64 u0x2199f06354c82d3b, ; 75: System.ClientModel.dll => 75
	i64 u0x219ea1b751a4dee4, ; 76: lib_System.IO.Compression.ZipFile.dll.so => 127
	i64 u0x21cc7e445dcd5469, ; 77: System.Reflection.Emit.ILGeneration => 151
	i64 u0x220fd4f2e7c48170, ; 78: th/Microsoft.Maui.Controls.resources => 27
	i64 u0x224538d85ed15a82, ; 79: System.IO.Pipes => 131
	i64 u0x237be844f1f812c7, ; 80: System.Threading.Thread.dll => 172
	i64 u0x23b0dd507a933aa9, ; 81: Google.Api.Gax => 38
	i64 u0x23e5bbaf425e0662, ; 82: lib_Anthropic.SDK.dll.so => 35
	i64 u0x2407aef2bbe8fadf, ; 83: System.Console => 118
	i64 u0x240abe014b27e7d3, ; 84: Xamarin.AndroidX.Core.dll => 85
	i64 u0x247619fe4413f8bf, ; 85: System.Runtime.Serialization.Primitives.dll => 159
	i64 u0x24b95d581a70fbee, ; 86: Grpc.Auth.dll => 49
	i64 u0x24d4238047d7310f, ; 87: Google.Apis.Auth => 41
	i64 u0x252073cc3caa62c2, ; 88: fr/Microsoft.Maui.Controls.resources.dll => 8
	i64 u0x2662c629b96b0b30, ; 89: lib_Xamarin.Kotlin.StdLib.dll.so => 105
	i64 u0x268c1439f13bcc29, ; 90: lib_Microsoft.Extensions.Primitives.dll.so => 64
	i64 u0x273f3515de5faf0d, ; 91: id/Microsoft.Maui.Controls.resources.dll => 13
	i64 u0x2742545f9094896d, ; 92: hr/Microsoft.Maui.Controls.resources => 11
	i64 u0x2759af78ab94d39b, ; 93: System.Net.WebSockets => 145
	i64 u0x27b410442fad6cf1, ; 94: Java.Interop.dll => 181
	i64 u0x2801845a2c71fbfb, ; 95: System.Net.Primitives.dll => 139
	i64 u0x2a128783efe70ba0, ; 96: uk/Microsoft.Maui.Controls.resources.dll => 29
	i64 u0x2a3b095612184159, ; 97: lib_System.Net.NetworkInformation.dll.so => 138
	i64 u0x2a6507a5ffabdf28, ; 98: System.Diagnostics.TraceSource.dll => 122
	i64 u0x2ad156c8e1354139, ; 99: fi/Microsoft.Maui.Controls.resources => 7
	i64 u0x2af298f63581d886, ; 100: System.Text.RegularExpressions.dll => 169
	i64 u0x2afc1c4f898552ee, ; 101: lib_System.Formats.Asn1.dll.so => 125
	i64 u0x2b148910ed40fbf9, ; 102: zh-Hant/Microsoft.Maui.Controls.resources.dll => 33
	i64 u0x2c8bd14bb93a7d82, ; 103: lib-pl-Microsoft.Maui.Controls.resources.dll.so => 20
	i64 u0x2cc9e1fed6257257, ; 104: lib_System.Reflection.Emit.Lightweight.dll.so => 152
	i64 u0x2cd723e9fe623c7c, ; 105: lib_System.Private.Xml.Linq.dll.so => 149
	i64 u0x2d169d318a968379, ; 106: System.Threading.dll => 173
	i64 u0x2d47774b7d993f59, ; 107: sv/Microsoft.Maui.Controls.resources.dll => 26
	i64 u0x2db915caf23548d2, ; 108: System.Text.Json.dll => 168
	i64 u0x2e5a40c319acb800, ; 109: System.IO.FileSystem => 129
	i64 u0x2e6f1f226821322a, ; 110: el/Microsoft.Maui.Controls.resources.dll => 5
	i64 u0x2f02f94df3200fe5, ; 111: System.Diagnostics.Process => 121
	i64 u0x2f2e98e1c89b1aff, ; 112: System.Xml.ReaderWriter => 176
	i64 u0x2ff49de6a71764a1, ; 113: lib_Microsoft.Extensions.Http.dll.so => 59
	i64 u0x309ee9eeec09a71e, ; 114: lib_Xamarin.AndroidX.Fragment.dll.so => 89
	i64 u0x31195fef5d8fb552, ; 115: _Microsoft.Android.Resource.Designer.dll => 34
	i64 u0x32243413e774362a, ; 116: Xamarin.AndroidX.CardView.dll => 82
	i64 u0x3235427f8d12dae1, ; 117: lib_System.Drawing.Primitives.dll.so => 123
	i64 u0x329753a17a517811, ; 118: fr/Microsoft.Maui.Controls.resources => 8
	i64 u0x32aa989ff07a84ff, ; 119: lib_System.Xml.ReaderWriter.dll.so => 176
	i64 u0x33a31443733849fe, ; 120: lib-es-Microsoft.Maui.Controls.resources.dll.so => 6
	i64 u0x33ec63a7e226adfb, ; 121: Google.Cloud.Location.dll => 44
	i64 u0x341abc357fbb4ebf, ; 122: lib_System.Net.Sockets.dll.so => 142
	i64 u0x34dfd74fe2afcf37, ; 123: Microsoft.Maui => 67
	i64 u0x34e292762d9615df, ; 124: cs/Microsoft.Maui.Controls.resources.dll => 2
	i64 u0x3508234247f48404, ; 125: Microsoft.Maui.Controls => 65
	i64 u0x353590da528c9d22, ; 126: System.ComponentModel.Annotations => 114
	i64 u0x3549870798b4cd30, ; 127: lib_Xamarin.AndroidX.ViewPager2.dll.so => 103
	i64 u0x355282fc1c909694, ; 128: Microsoft.Extensions.Configuration => 55
	i64 u0x36b2b50fdf589ae2, ; 129: System.Reflection.Emit.Lightweight => 152
	i64 u0x374ef46b06791af6, ; 130: System.Reflection.Primitives.dll => 153
	i64 u0x379e6c338e5508ad, ; 131: lib_Google.Api.Gax.Grpc.dll.so => 39
	i64 u0x385c17636bb6fe6e, ; 132: Xamarin.AndroidX.CustomView.dll => 87
	i64 u0x38869c811d74050e, ; 133: System.Net.NameResolution.dll => 137
	i64 u0x38fc7e5da9ea99e0, ; 134: Google.Cloud.Translate.V3.dll => 45
	i64 u0x393c226616977fdb, ; 135: lib_Xamarin.AndroidX.ViewPager.dll.so => 102
	i64 u0x395e37c3334cf82a, ; 136: lib-ca-Microsoft.Maui.Controls.resources.dll.so => 1
	i64 u0x39721dd6cab9d79e, ; 137: Polly.dll => 73
	i64 u0x39aa39fda111d9d3, ; 138: Newtonsoft.Json => 71
	i64 u0x39c7744f7b9dbcc9, ; 139: Mistral.SDK => 70
	i64 u0x3ab5859054645f72, ; 140: System.Security.Cryptography.Primitives.dll => 163
	i64 u0x3b860f9932505633, ; 141: lib_System.Text.Encoding.Extensions.dll.so => 166
	i64 u0x3c3aafb6b3a00bf6, ; 142: lib_System.Security.Cryptography.X509Certificates.dll.so => 164
	i64 u0x3c51334447dec9e7, ; 143: Google.LongRunning => 46
	i64 u0x3c7c495f58ac5ee9, ; 144: Xamarin.Kotlin.StdLib => 105
	i64 u0x3d46f0b995082740, ; 145: System.Xml.Linq => 175
	i64 u0x3d9c2a242b040a50, ; 146: lib_Xamarin.AndroidX.Core.dll.so => 85
	i64 u0x3daa14724d8f58e8, ; 147: Google.Protobuf.dll => 47
	i64 u0x3e027e6e728d7f1c, ; 148: Google.LongRunning.dll => 46
	i64 u0x3e707b1acaaea668, ; 149: lib_Polly.Extensions.Http.dll.so => 74
	i64 u0x3f510adf788828dd, ; 150: System.Threading.Tasks.Extensions => 171
	i64 u0x407a10bb4bf95829, ; 151: lib_Xamarin.AndroidX.Navigation.Common.dll.so => 95
	i64 u0x41406d6f37320d99, ; 152: Google.Api.Gax.Grpc.dll => 39
	i64 u0x41cab042be111c34, ; 153: lib_Xamarin.AndroidX.AppCompat.AppCompatResources.dll.so => 81
	i64 u0x4266c67fd9a4ee79, ; 154: Google.Api.CommonProtos => 37
	i64 u0x42d3cd7add035099, ; 155: System.Management.dll => 77
	i64 u0x42f9dcf63ba066b1, ; 156: lib_Mistral.SDK.dll.so => 70
	i64 u0x43375950ec7c1b6a, ; 157: netstandard.dll => 179
	i64 u0x434c4e1d9284cdae, ; 158: Mono.Android.dll => 183
	i64 u0x43950f84de7cc79a, ; 159: pl/Microsoft.Maui.Controls.resources.dll => 20
	i64 u0x448bd33429269b19, ; 160: Microsoft.CSharp => 109
	i64 u0x4499fa3c8e494654, ; 161: lib_System.Runtime.Serialization.Primitives.dll.so => 159
	i64 u0x4515080865a951a5, ; 162: Xamarin.Kotlin.StdLib.dll => 105
	i64 u0x45b31d67ff6f2b8a, ; 163: lib_Google.Apis.dll.so => 40
	i64 u0x45c40276a42e283e, ; 164: System.Diagnostics.TraceSource => 122
	i64 u0x46a4213bc97fe5ae, ; 165: lib-ru-Microsoft.Maui.Controls.resources.dll.so => 24
	i64 u0x47358bd471172e1d, ; 166: lib_System.Xml.Linq.dll.so => 175
	i64 u0x4747e19ad6a1d4bb, ; 167: Grpc.Net.Common => 52
	i64 u0x4787a936949fcac2, ; 168: System.Memory.Data.dll => 78
	i64 u0x47daf4e1afbada10, ; 169: pt/Microsoft.Maui.Controls.resources => 22
	i64 u0x49e952f19a4e2022, ; 170: System.ObjectModel => 147
	i64 u0x4a5667b2462a664b, ; 171: lib_Xamarin.AndroidX.Navigation.UI.dll.so => 98
	i64 u0x4b7b6532ded934b7, ; 172: System.Text.Json => 168
	i64 u0x4b8f8ea3c2df6bb0, ; 173: System.ClientModel => 75
	i64 u0x4cc5f15266470798, ; 174: lib_Xamarin.AndroidX.Loader.dll.so => 94
	i64 u0x4cf6f67dc77aacd2, ; 175: System.Net.NetworkInformation.dll => 138
	i64 u0x4d3183dd245425d4, ; 176: System.Net.WebSockets.Client.dll => 144
	i64 u0x4d479f968a05e504, ; 177: System.Linq.Expressions.dll => 132
	i64 u0x4d55a010ffc4faff, ; 178: System.Private.Xml => 150
	i64 u0x4d6001db23f8cd87, ; 179: lib_System.ClientModel.dll.so => 75
	i64 u0x4d95fccc1f67c7ca, ; 180: System.Runtime.Loader.dll => 156
	i64 u0x4dcf44c3c9b076a2, ; 181: it/Microsoft.Maui.Controls.resources.dll => 14
	i64 u0x4dd9247f1d2c3235, ; 182: Xamarin.AndroidX.Loader.dll => 94
	i64 u0x4e32f00cb0937401, ; 183: Mono.Android.Runtime => 182
	i64 u0x4ebd0c4b82c5eefc, ; 184: lib_System.Threading.Channels.dll.so => 170
	i64 u0x4f21ee6ef9eb527e, ; 185: ca/Microsoft.Maui.Controls.resources => 1
	i64 u0x5037f0be3c28c7a3, ; 186: lib_Microsoft.Maui.Controls.dll.so => 65
	i64 u0x508c1fa6b57728d9, ; 187: Grpc.Net.Common.dll => 52
	i64 u0x5131bbe80989093f, ; 188: Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll => 92
	i64 u0x51bb8a2afe774e32, ; 189: System.Drawing => 124
	i64 u0x526ce79eb8e90527, ; 190: lib_System.Net.Primitives.dll.so => 139
	i64 u0x5277169428c6ebf6, ; 191: lib_Grpc.Net.Common.dll.so => 52
	i64 u0x52829f00b4467c38, ; 192: lib_System.Data.Common.dll.so => 119
	i64 u0x529ffe06f39ab8db, ; 193: Xamarin.AndroidX.Core => 85
	i64 u0x52ff996554dbf352, ; 194: Microsoft.Maui.Graphics => 69
	i64 u0x533caebae0ecd4ce, ; 195: AI_Translator_Mobile_App => 108
	i64 u0x535f7e40e8fef8af, ; 196: lib-sk-Microsoft.Maui.Controls.resources.dll.so => 25
	i64 u0x53a96d5c86c9e194, ; 197: System.Net.NetworkInformation => 138
	i64 u0x53be1038a61e8d44, ; 198: System.Runtime.InteropServices.RuntimeInformation.dll => 154
	i64 u0x53c3014b9437e684, ; 199: lib-zh-HK-Microsoft.Maui.Controls.resources.dll.so => 31
	i64 u0x5435e6f049e9bc37, ; 200: System.Security.Claims.dll => 161
	i64 u0x54795225dd1587af, ; 201: lib_System.Runtime.dll.so => 160
	i64 u0x54b42cc2b8e65a84, ; 202: Google.Apis.Core.dll => 42
	i64 u0x556e8b63b660ab8b, ; 203: Xamarin.AndroidX.Lifecycle.Common.Jvm.dll => 90
	i64 u0x5588627c9a108ec9, ; 204: System.Collections.Specialized => 112
	i64 u0x564aee2d141e20b4, ; 205: Microsoft.Extensions.AI.Abstractions => 54
	i64 u0x56f76b6edb837f8b, ; 206: Polly => 73
	i64 u0x571c5cfbec5ae8e2, ; 207: System.Private.Uri => 148
	i64 u0x579a06fed6eec900, ; 208: System.Private.CoreLib.dll => 180
	i64 u0x57c542c14049b66d, ; 209: System.Diagnostics.DiagnosticSource => 120
	i64 u0x584e4fc21de1321a, ; 210: Anthropic.SDK => 35
	i64 u0x58601b2dda4a27b9, ; 211: lib-ja-Microsoft.Maui.Controls.resources.dll.so => 15
	i64 u0x58688d9af496b168, ; 212: Microsoft.Extensions.DependencyInjection.dll => 57
	i64 u0x595a356d23e8da9a, ; 213: lib_Microsoft.CSharp.dll.so => 109
	i64 u0x59a935a032dbc08c, ; 214: lib_Grpc.Auth.dll.so => 49
	i64 u0x5a70033ca9d003cb, ; 215: lib_System.Memory.Data.dll.so => 78
	i64 u0x5a89a886ae30258d, ; 216: lib_Xamarin.AndroidX.CoordinatorLayout.dll.so => 84
	i64 u0x5a8f6699f4a1caa9, ; 217: lib_System.Threading.dll.so => 173
	i64 u0x5aaa5f93afcdd273, ; 218: lib_DeepL.net.dll.so => 36
	i64 u0x5ae9cd33b15841bf, ; 219: System.ComponentModel => 117
	i64 u0x5b5f0e240a06a2a2, ; 220: da/Microsoft.Maui.Controls.resources.dll => 3
	i64 u0x5c393624b8176517, ; 221: lib_Microsoft.Extensions.Logging.dll.so => 61
	i64 u0x5d0a4a29b02d9d3c, ; 222: System.Net.WebHeaderCollection.dll => 143
	i64 u0x5db0cbbd1028510e, ; 223: lib_System.Runtime.InteropServices.dll.so => 155
	i64 u0x5db30905d3e5013b, ; 224: Xamarin.AndroidX.Collection.Jvm.dll => 83
	i64 u0x5e467bc8f09ad026, ; 225: System.Collections.Specialized.dll => 112
	i64 u0x5ea92fdb19ec8c4c, ; 226: System.Text.Encodings.Web.dll => 167
	i64 u0x5eb8046dd40e9ac3, ; 227: System.ComponentModel.Primitives => 115
	i64 u0x5eee1376d94c7f5e, ; 228: System.Net.HttpListener.dll => 136
	i64 u0x5f2225e69bf082b9, ; 229: OpenAI.dll => 72
	i64 u0x5f36ccf5c6a57e24, ; 230: System.Xml.ReaderWriter.dll => 176
	i64 u0x5f4294b9b63cb842, ; 231: System.Data.Common => 119
	i64 u0x5f9a2d823f664957, ; 232: lib-el-Microsoft.Maui.Controls.resources.dll.so => 5
	i64 u0x609f4b7b63d802d4, ; 233: lib_Microsoft.Extensions.DependencyInjection.dll.so => 57
	i64 u0x60cd4e33d7e60134, ; 234: Xamarin.KotlinX.Coroutines.Core.Jvm => 106
	i64 u0x60f62d786afcf130, ; 235: System.Memory => 134
	i64 u0x61be8d1299194243, ; 236: Microsoft.Maui.Controls.Xaml => 66
	i64 u0x61d2cba29557038f, ; 237: de/Microsoft.Maui.Controls.resources => 4
	i64 u0x61d88f399afb2f45, ; 238: lib_System.Runtime.Loader.dll.so => 156
	i64 u0x622eef6f9e59068d, ; 239: System.Private.CoreLib => 180
	i64 u0x6400f68068c1e9f1, ; 240: Xamarin.Google.Android.Material.dll => 104
	i64 u0x640e3b14dbd325c2, ; 241: System.Security.Cryptography.Algorithms.dll => 162
	i64 u0x65ecac39144dd3cc, ; 242: Microsoft.Maui.Controls.dll => 65
	i64 u0x65ece51227bfa724, ; 243: lib_System.Runtime.Numerics.dll.so => 157
	i64 u0x6692e924eade1b29, ; 244: lib_System.Console.dll.so => 118
	i64 u0x66a4e5c6a3fb0bae, ; 245: lib_Xamarin.AndroidX.Lifecycle.ViewModel.Android.dll.so => 92
	i64 u0x66d13304ce1a3efa, ; 246: Xamarin.AndroidX.CursorAdapter => 86
	i64 u0x68558ec653afa616, ; 247: lib-da-Microsoft.Maui.Controls.resources.dll.so => 3
	i64 u0x6872ec7a2e36b1ac, ; 248: System.Drawing.Primitives.dll => 123
	i64 u0x68fbbbe2eb455198, ; 249: System.Formats.Asn1 => 125
	i64 u0x69063fc0ba8e6bdd, ; 250: he/Microsoft.Maui.Controls.resources.dll => 9
	i64 u0x6a4d7577b2317255, ; 251: System.Runtime.InteropServices.dll => 155
	i64 u0x6ace3b74b15ee4a4, ; 252: nb/Microsoft.Maui.Controls.resources => 18
	i64 u0x6bc822f45373a1d6, ; 253: Google.Apis.dll => 40
	i64 u0x6d12bfaa99c72b1f, ; 254: lib_Microsoft.Maui.Graphics.dll.so => 69
	i64 u0x6d79993361e10ef2, ; 255: Microsoft.Extensions.Primitives => 64
	i64 u0x6d86d56b84c8eb71, ; 256: lib_Xamarin.AndroidX.CursorAdapter.dll.so => 86
	i64 u0x6d9bea6b3e895cf7, ; 257: Microsoft.Extensions.Primitives.dll => 64
	i64 u0x6e25a02c3833319a, ; 258: lib_Xamarin.AndroidX.Navigation.Fragment.dll.so => 96
	i64 u0x6fd2265da78b93a4, ; 259: lib_Microsoft.Maui.dll.so => 67
	i64 u0x6fdfc7de82c33008, ; 260: cs/Microsoft.Maui.Controls.resources => 2
	i64 u0x6fe1b892f5da8856, ; 261: lib_Microsoft.Extensions.AI.Abstractions.dll.so => 54
	i64 u0x701cd46a1c25a5fe, ; 262: System.IO.FileSystem.dll => 129
	i64 u0x70e99f48c05cb921, ; 263: tr/Microsoft.Maui.Controls.resources.dll => 28
	i64 u0x70fd3deda22442d2, ; 264: lib-nb-Microsoft.Maui.Controls.resources.dll.so => 18
	i64 u0x71a495ea3761dde8, ; 265: lib-it-Microsoft.Maui.Controls.resources.dll.so => 14
	i64 u0x71ad672adbe48f35, ; 266: System.ComponentModel.Primitives.dll => 115
	i64 u0x72b1fb4109e08d7b, ; 267: lib-hr-Microsoft.Maui.Controls.resources.dll.so => 11
	i64 u0x730bfb248998f67a, ; 268: System.IO.Compression.ZipFile => 127
	i64 u0x73e4ce94e2eb6ffc, ; 269: lib_System.Memory.dll.so => 134
	i64 u0x74f1d6f5ab554b1d, ; 270: Microsoft.Extensions.Http.Polly.dll => 60
	i64 u0x755a91767330b3d4, ; 271: lib_Microsoft.Extensions.Configuration.dll.so => 55
	i64 u0x76012e7334db86e5, ; 272: lib_Xamarin.AndroidX.SavedState.dll.so => 100
	i64 u0x76ca07b878f44da0, ; 273: System.Runtime.Numerics.dll => 157
	i64 u0x780bc73597a503a9, ; 274: lib-ms-Microsoft.Maui.Controls.resources.dll.so => 17
	i64 u0x783606d1e53e7a1a, ; 275: th/Microsoft.Maui.Controls.resources.dll => 27
	i64 u0x78a45e51311409b6, ; 276: Xamarin.AndroidX.Fragment.dll => 89
	i64 u0x7a25bdb29108c6e7, ; 277: Microsoft.Extensions.Http => 59
	i64 u0x7adb8da2ac89b647, ; 278: fi/Microsoft.Maui.Controls.resources.dll => 7
	i64 u0x7bef86a4335c4870, ; 279: System.ComponentModel.TypeConverter => 116
	i64 u0x7c0820144cd34d6a, ; 280: sk/Microsoft.Maui.Controls.resources.dll => 25
	i64 u0x7c2a0bd1e0f988fc, ; 281: lib-de-Microsoft.Maui.Controls.resources.dll.so => 4
	i64 u0x7d649b75d580bb42, ; 282: ms/Microsoft.Maui.Controls.resources.dll => 17
	i64 u0x7d8ee2bdc8e3aad1, ; 283: System.Numerics.Vectors => 146
	i64 u0x7dfc3d6d9d8d7b70, ; 284: System.Collections => 113
	i64 u0x7e302e110e1e1346, ; 285: lib_System.Security.Claims.dll.so => 161
	i64 u0x7e946809d6008ef2, ; 286: lib_System.ObjectModel.dll.so => 147
	i64 u0x7ecc13347c8fd849, ; 287: lib_System.ComponentModel.dll.so => 117
	i64 u0x7f00ddd9b9ca5a13, ; 288: Xamarin.AndroidX.ViewPager.dll => 102
	i64 u0x7f9351cd44b1273f, ; 289: Microsoft.Extensions.Configuration.Abstractions => 56
	i64 u0x7fbd557c99b3ce6f, ; 290: lib_Xamarin.AndroidX.Lifecycle.LiveData.Core.dll.so => 91
	i64 u0x812c069d5cdecc17, ; 291: System.dll => 178
	i64 u0x81ab745f6c0f5ce6, ; 292: zh-Hant/Microsoft.Maui.Controls.resources => 33
	i64 u0x8277f2be6b5ce05f, ; 293: Xamarin.AndroidX.AppCompat => 80
	i64 u0x828f06563b30bc50, ; 294: lib_Xamarin.AndroidX.CardView.dll.so => 82
	i64 u0x82df8f5532a10c59, ; 295: lib_System.Drawing.dll.so => 124
	i64 u0x82f6403342e12049, ; 296: uk/Microsoft.Maui.Controls.resources => 29
	i64 u0x83c14ba66c8e2b8c, ; 297: zh-Hans/Microsoft.Maui.Controls.resources => 32
	i64 u0x84ae73148a4557d2, ; 298: lib_System.IO.Pipes.dll.so => 131
	i64 u0x86a909228dc7657b, ; 299: lib-zh-Hant-Microsoft.Maui.Controls.resources.dll.so => 33
	i64 u0x86b3e00c36b84509, ; 300: Microsoft.Extensions.Configuration.dll => 55
	i64 u0x87c69b87d9283884, ; 301: lib_System.Threading.Thread.dll.so => 172
	i64 u0x87f6569b25707834, ; 302: System.IO.Compression.Brotli.dll => 126
	i64 u0x87fef727071b7fe5, ; 303: Grpc.Net.Client => 51
	i64 u0x8842b3a5d2d3fb36, ; 304: Microsoft.Maui.Essentials => 68
	i64 u0x88bda98e0cffb7a9, ; 305: lib_Xamarin.KotlinX.Coroutines.Core.Jvm.dll.so => 106
	i64 u0x8930322c7bd8f768, ; 306: netstandard => 179
	i64 u0x897a606c9e39c75f, ; 307: lib_System.ComponentModel.Primitives.dll.so => 115
	i64 u0x89c5188089ec2cd5, ; 308: lib_System.Runtime.InteropServices.RuntimeInformation.dll.so => 154
	i64 u0x8ad229ea26432ee2, ; 309: Xamarin.AndroidX.Loader => 94
	i64 u0x8b4ff5d0fdd5faa1, ; 310: lib_System.Diagnostics.DiagnosticSource.dll.so => 120
	i64 u0x8b8d01333a96d0b5, ; 311: System.Diagnostics.Process.dll => 121
	i64 u0x8b9ceca7acae3451, ; 312: lib-he-Microsoft.Maui.Controls.resources.dll.so => 9
	i64 u0x8cef633b2591563f, ; 313: DeepL.net => 36
	i64 u0x8d0f420977c2c1c7, ; 314: Xamarin.AndroidX.CursorAdapter.dll => 86
	i64 u0x8d7b8ab4b3310ead, ; 315: System.Threading => 173
	i64 u0x8da188285aadfe8e, ; 316: System.Collections.Concurrent => 110
	i64 u0x8dfc1cfbf8858f95, ; 317: Grpc.Core.Api.dll => 50
	i64 u0x8ec6e06a61c1baeb, ; 318: lib_Newtonsoft.Json.dll.so => 71
	i64 u0x8ed807bfe9858dfc, ; 319: Xamarin.AndroidX.Navigation.Common => 95
	i64 u0x8ee08b8194a30f48, ; 320: lib-hi-Microsoft.Maui.Controls.resources.dll.so => 10
	i64 u0x8ef7601039857a44, ; 321: lib-ro-Microsoft.Maui.Controls.resources.dll.so => 23
	i64 u0x8f32c6f611f6ffab, ; 322: pt/Microsoft.Maui.Controls.resources.dll => 22
	i64 u0x8f8829d21c8985a4, ; 323: lib-pt-BR-Microsoft.Maui.Controls.resources.dll.so => 21
	i64 u0x90263f8448b8f572, ; 324: lib_System.Diagnostics.TraceSource.dll.so => 122
	i64 u0x903101b46fb73a04, ; 325: _Microsoft.Android.Resource.Designer => 34
	i64 u0x90393bd4865292f3, ; 326: lib_System.IO.Compression.dll.so => 128
	i64 u0x905e2b8e7ae91ae6, ; 327: System.Threading.Tasks.Extensions.dll => 171
	i64 u0x90634f86c5ebe2b5, ; 328: Xamarin.AndroidX.Lifecycle.ViewModel.Android => 92
	i64 u0x907b636704ad79ef, ; 329: lib_Microsoft.Maui.Controls.Xaml.dll.so => 66
	i64 u0x91418dc638b29e68, ; 330: lib_Xamarin.AndroidX.CustomView.dll.so => 87
	i64 u0x9157bd523cd7ed36, ; 331: lib_System.Text.Json.dll.so => 168
	i64 u0x916a9a7f37120c4b, ; 332: Microsoft.Extensions.AI.Abstractions.dll => 54
	i64 u0x91a74f07b30d37e2, ; 333: System.Linq.dll => 133
	i64 u0x91fa41a87223399f, ; 334: ca/Microsoft.Maui.Controls.resources.dll => 1
	i64 u0x93cfa73ab28d6e35, ; 335: ms/Microsoft.Maui.Controls.resources => 17
	i64 u0x944077d8ca3c6580, ; 336: System.IO.Compression.dll => 128
	i64 u0x967fc325e09bfa8c, ; 337: es/Microsoft.Maui.Controls.resources => 6
	i64 u0x9729c8c4c069c478, ; 338: Google.Apis.Core => 42
	i64 u0x9732d8dbddea3d9a, ; 339: id/Microsoft.Maui.Controls.resources => 13
	i64 u0x978be80e5210d31b, ; 340: Microsoft.Maui.Graphics.dll => 69
	i64 u0x97b8c771ea3e4220, ; 341: System.ComponentModel.dll => 117
	i64 u0x97e144c9d3c6976e, ; 342: System.Collections.Concurrent.dll => 110
	i64 u0x991d510397f92d9d, ; 343: System.Linq.Expressions => 132
	i64 u0x99a00ca5270c6878, ; 344: Xamarin.AndroidX.Navigation.Runtime => 97
	i64 u0x99cdc6d1f2d3a72f, ; 345: ko/Microsoft.Maui.Controls.resources.dll => 16
	i64 u0x9c244ac7cda32d26, ; 346: System.Security.Cryptography.X509Certificates.dll => 164
	i64 u0x9d052eb79c53b587, ; 347: lib_Polly.dll.so => 73
	i64 u0x9d5dbcf5a48583fe, ; 348: lib_Xamarin.AndroidX.Activity.dll.so => 79
	i64 u0x9d74dee1a7725f34, ; 349: Microsoft.Extensions.Configuration.Abstractions.dll => 56
	i64 u0x9e4534b6adaf6e84, ; 350: nl/Microsoft.Maui.Controls.resources => 19
	i64 u0x9eaf1efdf6f7267e, ; 351: Xamarin.AndroidX.Navigation.Common.dll => 95
	i64 u0x9ee6ffcd8a354b81, ; 352: Polly.Extensions.Http.dll => 74
	i64 u0x9ef542cf1f78c506, ; 353: Xamarin.AndroidX.Lifecycle.LiveData.Core => 91
	i64 u0xa0d8259f4cc284ec, ; 354: lib_System.Security.Cryptography.dll.so => 165
	i64 u0xa1440773ee9d341e, ; 355: Xamarin.Google.Android.Material => 104
	i64 u0xa1b9d7c27f47219f, ; 356: Xamarin.AndroidX.Navigation.UI.dll => 98
	i64 u0xa2572680829d2c7c, ; 357: System.IO.Pipelines.dll => 130
	i64 u0xa29a8c6cae004eb6, ; 358: Google.Cloud.Iam.V1 => 43
	i64 u0xa41cb07c12522387, ; 359: lib_Google.Cloud.Translate.V3.dll.so => 45
	i64 u0xa46aa1eaa214539b, ; 360: ko/Microsoft.Maui.Controls.resources => 16
	i64 u0xa4edc8f2ceae241a, ; 361: System.Data.Common.dll => 119
	i64 u0xa5494f40f128ce6a, ; 362: System.Runtime.Serialization.Formatters.dll => 158
	i64 u0xa5e599d1e0524750, ; 363: System.Numerics.Vectors.dll => 146
	i64 u0xa5f1ba49b85dd355, ; 364: System.Security.Cryptography.dll => 165
	i64 u0xa67dbee13e1df9ca, ; 365: Xamarin.AndroidX.SavedState.dll => 100
	i64 u0xa68a420042bb9b1f, ; 366: Xamarin.AndroidX.DrawerLayout.dll => 88
	i64 u0xa78ce3745383236a, ; 367: Xamarin.AndroidX.Lifecycle.Common.Jvm => 90
	i64 u0xa7c31b56b4dc7b33, ; 368: hu/Microsoft.Maui.Controls.resources => 12
	i64 u0xa952cc4a0d808a59, ; 369: lib_Google.Api.CommonProtos.dll.so => 37
	i64 u0xa97144612a412894, ; 370: Google.Cloud.Translate.V3 => 45
	i64 u0xaa2219c8e3449ff5, ; 371: Microsoft.Extensions.Logging.Abstractions => 62
	i64 u0xaa311f6655bd297d, ; 372: lib_Google.Cloud.Iam.V1.dll.so => 43
	i64 u0xaa443ac34067eeef, ; 373: System.Private.Xml.dll => 150
	i64 u0xaa52de307ef5d1dd, ; 374: System.Net.Http => 135
	i64 u0xaaaf86367285a918, ; 375: Microsoft.Extensions.DependencyInjection.Abstractions.dll => 58
	i64 u0xaaf84bb3f052a265, ; 376: el/Microsoft.Maui.Controls.resources => 5
	i64 u0xab9c1b2687d86b0b, ; 377: lib_System.Linq.Expressions.dll.so => 132
	i64 u0xac2af3fa195a15ce, ; 378: System.Runtime.Numerics => 157
	i64 u0xac5376a2a538dc10, ; 379: Xamarin.AndroidX.Lifecycle.LiveData.Core.dll => 91
	i64 u0xac65e40f62b6b90e, ; 380: Google.Protobuf => 47
	i64 u0xac98d31068e24591, ; 381: System.Xml.XDocument => 177
	i64 u0xacd46e002c3ccb97, ; 382: ro/Microsoft.Maui.Controls.resources => 23
	i64 u0xacf42eea7ef9cd12, ; 383: System.Threading.Channels => 170
	i64 u0xad89c07347f1bad6, ; 384: nl/Microsoft.Maui.Controls.resources.dll => 19
	i64 u0xadbb53caf78a79d2, ; 385: System.Web.HttpUtility => 174
	i64 u0xadc90ab061a9e6e4, ; 386: System.ComponentModel.TypeConverter.dll => 116
	i64 u0xadf511667bef3595, ; 387: System.Net.Security => 141
	i64 u0xae282bcd03739de7, ; 388: Java.Interop => 181
	i64 u0xae53579c90db1107, ; 389: System.ObjectModel.dll => 147
	i64 u0xafe29f45095518e7, ; 390: lib_Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll.so => 93
	i64 u0xb05cc42cd94c6d9d, ; 391: lib-sv-Microsoft.Maui.Controls.resources.dll.so => 26
	i64 u0xb220631954820169, ; 392: System.Text.RegularExpressions => 169
	i64 u0xb2a3f67f3bf29fce, ; 393: da/Microsoft.Maui.Controls.resources => 3
	i64 u0xb39eed1decc0cd95, ; 394: Google.Api.Gax.dll => 38
	i64 u0xb3f0a0fcda8d3ebc, ; 395: Xamarin.AndroidX.CardView => 82
	i64 u0xb4512edf6d2b372b, ; 396: Google.Cloud.Location => 44
	i64 u0xb46be1aa6d4fff93, ; 397: hi/Microsoft.Maui.Controls.resources => 10
	i64 u0xb477491be13109d8, ; 398: ar/Microsoft.Maui.Controls.resources => 0
	i64 u0xb4bd7015ecee9d86, ; 399: System.IO.Pipelines => 130
	i64 u0xb5c7fcdafbc67ee4, ; 400: Microsoft.Extensions.Logging.Abstractions.dll => 62
	i64 u0xb7212c4683a94afe, ; 401: System.Drawing.Primitives => 123
	i64 u0xb7b7753d1f319409, ; 402: sv/Microsoft.Maui.Controls.resources => 26
	i64 u0xb81a2c6e0aee50fe, ; 403: lib_System.Private.CoreLib.dll.so => 180
	i64 u0xb872c26142d22aa9, ; 404: Microsoft.Extensions.Http.dll => 59
	i64 u0xb898d1802c1a108c, ; 405: lib_System.Management.dll.so => 77
	i64 u0xb90ff82c284e9af9, ; 406: Grpc.Core.Api => 50
	i64 u0xb9185c33a1643eed, ; 407: Microsoft.CSharp.dll => 109
	i64 u0xb9f64d3b230def68, ; 408: lib-pt-Microsoft.Maui.Controls.resources.dll.so => 22
	i64 u0xb9fc3c8a556e3691, ; 409: ja/Microsoft.Maui.Controls.resources => 15
	i64 u0xba4670aa94a2b3c6, ; 410: lib_System.Xml.XDocument.dll.so => 177
	i64 u0xba48785529705af9, ; 411: System.Collections.dll => 113
	i64 u0xbb6026d73f757bcf, ; 412: Google.Api.Gax.Grpc => 39
	i64 u0xbb65706fde942ce3, ; 413: System.Net.Sockets => 142
	i64 u0xbbd180354b67271a, ; 414: System.Runtime.Serialization.Formatters => 158
	i64 u0xbc58c80728da3fd2, ; 415: GenerativeAI.dll => 48
	i64 u0xbd0e2c0d55246576, ; 416: System.Net.Http.dll => 135
	i64 u0xbd3fbd85b9e1cb29, ; 417: lib_System.Net.HttpListener.dll.so => 136
	i64 u0xbd437a2cdb333d0d, ; 418: Xamarin.AndroidX.ViewPager2 => 103
	i64 u0xbd4f572d2bd0a789, ; 419: System.IO.Compression.ZipFile.dll => 127
	i64 u0xbee38d4a88835966, ; 420: Xamarin.AndroidX.AppCompat.AppCompatResources => 81
	i64 u0xc040a4ab55817f58, ; 421: ar/Microsoft.Maui.Controls.resources.dll => 0
	i64 u0xc0d928351ab5ca77, ; 422: System.Console.dll => 118
	i64 u0xc12b8b3afa48329c, ; 423: lib_System.Linq.dll.so => 133
	i64 u0xc1649f545b2f76aa, ; 424: Grpc.Auth => 49
	i64 u0xc1ff9ae3cdb6e1e6, ; 425: Xamarin.AndroidX.Activity.dll => 79
	i64 u0xc2850fbba221599d, ; 426: lib_Google.Apis.Core.dll.so => 42
	i64 u0xc28c50f32f81cc73, ; 427: ja/Microsoft.Maui.Controls.resources.dll => 15
	i64 u0xc2bcfec99f69365e, ; 428: Xamarin.AndroidX.ViewPager2.dll => 103
	i64 u0xc421b61fd853169d, ; 429: lib_System.Net.WebSockets.Client.dll.so => 144
	i64 u0xc4d3858ed4d08512, ; 430: Xamarin.AndroidX.Lifecycle.ViewModelSavedState.dll => 93
	i64 u0xc50fded0ded1418c, ; 431: lib_System.ComponentModel.TypeConverter.dll.so => 116
	i64 u0xc519125d6bc8fb11, ; 432: lib_System.Net.Requests.dll.so => 140
	i64 u0xc5293b19e4dc230e, ; 433: Xamarin.AndroidX.Navigation.Fragment => 96
	i64 u0xc5325b2fcb37446f, ; 434: lib_System.Private.Xml.dll.so => 150
	i64 u0xc5a0f4b95a699af7, ; 435: lib_System.Private.Uri.dll.so => 148
	i64 u0xc5cdcd5b6277579e, ; 436: lib_System.Security.Cryptography.Algorithms.dll.so => 162
	i64 u0xc5d608afb58abba2, ; 437: Google.Apis.Auth.dll => 41
	i64 u0xc7c01e7d7c93a110, ; 438: System.Text.Encoding.Extensions.dll => 166
	i64 u0xc7ce851898a4548e, ; 439: lib_System.Web.HttpUtility.dll.so => 174
	i64 u0xc858a28d9ee5a6c5, ; 440: lib_System.Collections.Specialized.dll.so => 112
	i64 u0xca3a723e7342c5b6, ; 441: lib-tr-Microsoft.Maui.Controls.resources.dll.so => 28
	i64 u0xcab3493c70141c2d, ; 442: pl/Microsoft.Maui.Controls.resources => 20
	i64 u0xcacfddc9f7c6de76, ; 443: ro/Microsoft.Maui.Controls.resources.dll => 23
	i64 u0xcbd4fdd9cef4a294, ; 444: lib__Microsoft.Android.Resource.Designer.dll.so => 34
	i64 u0xcc182c3afdc374d6, ; 445: Microsoft.Bcl.AsyncInterfaces => 53
	i64 u0xcc2876b32ef2794c, ; 446: lib_System.Text.RegularExpressions.dll.so => 169
	i64 u0xcc5c3bb714c4561e, ; 447: Xamarin.KotlinX.Coroutines.Core.Jvm.dll => 106
	i64 u0xcc76886e09b88260, ; 448: Xamarin.KotlinX.Serialization.Core.Jvm.dll => 107
	i64 u0xccf25c4b634ccd3a, ; 449: zh-Hans/Microsoft.Maui.Controls.resources.dll => 32
	i64 u0xcd10a42808629144, ; 450: System.Net.Requests => 140
	i64 u0xcdd0c48b6937b21c, ; 451: Xamarin.AndroidX.SwipeRefreshLayout => 101
	i64 u0xcf23d8093f3ceadf, ; 452: System.Diagnostics.DiagnosticSource.dll => 120
	i64 u0xcf8fc898f98b0d34, ; 453: System.Private.Xml.Linq => 149
	i64 u0xd1194e1d8a8de83c, ; 454: lib_Xamarin.AndroidX.Lifecycle.Common.Jvm.dll.so => 90
	i64 u0xd333d0af9e423810, ; 455: System.Runtime.InteropServices => 155
	i64 u0xd3426d966bb704f5, ; 456: Xamarin.AndroidX.AppCompat.AppCompatResources.dll => 81
	i64 u0xd3651b6fc3125825, ; 457: System.Private.Uri.dll => 148
	i64 u0xd373685349b1fe8b, ; 458: Microsoft.Extensions.Logging.dll => 61
	i64 u0xd3e4c8d6a2d5d470, ; 459: it/Microsoft.Maui.Controls.resources => 14
	i64 u0xd4645626dffec99d, ; 460: lib_Microsoft.Extensions.DependencyInjection.Abstractions.dll.so => 58
	i64 u0xd5507e11a2b2839f, ; 461: Xamarin.AndroidX.Lifecycle.ViewModelSavedState => 93
	i64 u0xd64f50eb4ba264b3, ; 462: lib_Google.LongRunning.dll.so => 46
	i64 u0xd6694f8359737e4e, ; 463: Xamarin.AndroidX.SavedState => 100
	i64 u0xd6d21782156bc35b, ; 464: Xamarin.AndroidX.SwipeRefreshLayout.dll => 101
	i64 u0xd72329819cbbbc44, ; 465: lib_Microsoft.Extensions.Configuration.Abstractions.dll.so => 56
	i64 u0xd7b3764ada9d341d, ; 466: lib_Microsoft.Extensions.Logging.Abstractions.dll.so => 62
	i64 u0xd8113d9a7e8ad136, ; 467: System.CodeDom => 76
	i64 u0xd831c46ee29e011f, ; 468: lib_AI_Translator_Mobile_App.dll.so => 108
	i64 u0xda1dfa4c534a9251, ; 469: Microsoft.Extensions.DependencyInjection => 57
	i64 u0xdad05a11827959a3, ; 470: System.Collections.NonGeneric.dll => 111
	i64 u0xdb5383ab5865c007, ; 471: lib-vi-Microsoft.Maui.Controls.resources.dll.so => 30
	i64 u0xdb58816721c02a59, ; 472: lib_System.Reflection.Emit.ILGeneration.dll.so => 151
	i64 u0xdbeda89f832aa805, ; 473: vi/Microsoft.Maui.Controls.resources.dll => 30
	i64 u0xdbf9607a441b4505, ; 474: System.Linq => 133
	i64 u0xdcbd21904ff0f297, ; 475: Google.Apis => 40
	i64 u0xdce2c53525640bf3, ; 476: Microsoft.Extensions.Logging => 61
	i64 u0xdd2b722d78ef5f43, ; 477: System.Runtime.dll => 160
	i64 u0xdd67031857c72f96, ; 478: lib_System.Text.Encodings.Web.dll.so => 167
	i64 u0xdde30e6b77aa6f6c, ; 479: lib-zh-Hans-Microsoft.Maui.Controls.resources.dll.so => 32
	i64 u0xddf3ef21cad250f2, ; 480: lib_OpenAI.dll.so => 72
	i64 u0xde110ae80fa7c2e2, ; 481: System.Xml.XDocument.dll => 177
	i64 u0xde572c2b2fb32f93, ; 482: lib_System.Threading.Tasks.Extensions.dll.so => 171
	i64 u0xde7bd7eebc2484cc, ; 483: lib_GenerativeAI.dll.so => 48
	i64 u0xde8769ebda7d8647, ; 484: hr/Microsoft.Maui.Controls.resources.dll => 11
	i64 u0xe0142572c095a480, ; 485: Xamarin.AndroidX.AppCompat.dll => 80
	i64 u0xe02f89350ec78051, ; 486: Xamarin.AndroidX.CoordinatorLayout.dll => 84
	i64 u0xe10b760bb1462e7a, ; 487: lib_System.Security.Cryptography.Primitives.dll.so => 163
	i64 u0xe192a588d4410686, ; 488: lib_System.IO.Pipelines.dll.so => 130
	i64 u0xe1a08bd3fa539e0d, ; 489: System.Runtime.Loader => 156
	i64 u0xe1b52f9f816c70ef, ; 490: System.Private.Xml.Linq.dll => 149
	i64 u0xe1ecfdb7fff86067, ; 491: System.Net.Security.dll => 141
	i64 u0xe2420585aeceb728, ; 492: System.Net.Requests.dll => 140
	i64 u0xe29b73bc11392966, ; 493: lib-id-Microsoft.Maui.Controls.resources.dll.so => 13
	i64 u0xe3811d68d4fe8463, ; 494: pt-BR/Microsoft.Maui.Controls.resources.dll => 21
	i64 u0xe494f7ced4ecd10a, ; 495: hu/Microsoft.Maui.Controls.resources.dll => 12
	i64 u0xe49a982a2533a332, ; 496: lib_Google.Cloud.Location.dll.so => 44
	i64 u0xe4a9b1e40d1e8917, ; 497: lib-fi-Microsoft.Maui.Controls.resources.dll.so => 7
	i64 u0xe4f74a0b5bf9703f, ; 498: System.Runtime.Serialization.Primitives => 159
	i64 u0xe5434e8a119ceb69, ; 499: lib_Mono.Android.dll.so => 183
	i64 u0xe5876071b3482131, ; 500: Polly.Extensions.Http => 74
	i64 u0xe6e77c648688b75b, ; 501: Google.Api.CommonProtos.dll => 37
	i64 u0xe89a2a9ef110899b, ; 502: System.Drawing.dll => 124
	i64 u0xe98b0e4b4d44e931, ; 503: lib_Grpc.Net.Client.dll.so => 51
	i64 u0xeaf8e9970fc2fe69, ; 504: System.Management => 77
	i64 u0xebd1e72ce842c9d6, ; 505: DeepL.net.dll => 36
	i64 u0xedc4817167106c23, ; 506: System.Net.Sockets.dll => 142
	i64 u0xedc632067fb20ff3, ; 507: System.Memory.dll => 134
	i64 u0xedc8e4ca71a02a8b, ; 508: Xamarin.AndroidX.Navigation.Runtime.dll => 97
	i64 u0xee46012b76c17677, ; 509: Mistral.SDK.dll => 70
	i64 u0xeeb7ebb80150501b, ; 510: lib_Xamarin.AndroidX.Collection.Jvm.dll.so => 83
	i64 u0xef72742e1bcca27a, ; 511: Microsoft.Maui.Essentials.dll => 68
	i64 u0xefec0b7fdc57ec42, ; 512: Xamarin.AndroidX.Activity => 79
	i64 u0xf008bcd238ede2c8, ; 513: System.CodeDom.dll => 76
	i64 u0xf00c29406ea45e19, ; 514: es/Microsoft.Maui.Controls.resources.dll => 6
	i64 u0xf09e47b6ae914f6e, ; 515: System.Net.NameResolution => 137
	i64 u0xf0de2537ee19c6ca, ; 516: lib_System.Net.WebHeaderCollection.dll.so => 143
	i64 u0xf11b621fc87b983f, ; 517: Microsoft.Maui.Controls.Xaml.dll => 66
	i64 u0xf1c4b4005493d871, ; 518: System.Formats.Asn1.dll => 125
	i64 u0xf238bd79489d3a96, ; 519: lib-nl-Microsoft.Maui.Controls.resources.dll.so => 19
	i64 u0xf37221fda4ef8830, ; 520: lib_Xamarin.Google.Android.Material.dll.so => 104
	i64 u0xf3ddfe05336abf29, ; 521: System => 178
	i64 u0xf408654b2a135055, ; 522: System.Reflection.Emit.ILGeneration.dll => 151
	i64 u0xf42716c04938101c, ; 523: Google.Cloud.Iam.V1.dll => 43
	i64 u0xf4c1dd70a5496a17, ; 524: System.IO.Compression => 128
	i64 u0xf5fc7602fe27b333, ; 525: System.Net.WebHeaderCollection => 143
	i64 u0xf6077741019d7428, ; 526: Xamarin.AndroidX.CoordinatorLayout => 84
	i64 u0xf77b20923f07c667, ; 527: de/Microsoft.Maui.Controls.resources.dll => 4
	i64 u0xf7e2cac4c45067b3, ; 528: lib_System.Numerics.Vectors.dll.so => 146
	i64 u0xf7e74930e0e3d214, ; 529: zh-HK/Microsoft.Maui.Controls.resources.dll => 31
	i64 u0xf7fa0bf77fe677cc, ; 530: Newtonsoft.Json.dll => 71
	i64 u0xf84773b5c81e3cef, ; 531: lib-uk-Microsoft.Maui.Controls.resources.dll.so => 29
	i64 u0xf87e0d5910d5cb96, ; 532: lib_Microsoft.Extensions.Http.Polly.dll.so => 60
	i64 u0xf8b77539b362d3ba, ; 533: lib_System.Reflection.Primitives.dll.so => 153
	i64 u0xf8e045dc345b2ea3, ; 534: lib_Xamarin.AndroidX.RecyclerView.dll.so => 99
	i64 u0xf915dc29808193a1, ; 535: System.Web.HttpUtility.dll => 174
	i64 u0xf96c777a2a0686f4, ; 536: hi/Microsoft.Maui.Controls.resources.dll => 10
	i64 u0xf9eec5bb3a6aedc6, ; 537: Microsoft.Extensions.Options => 63
	i64 u0xfa3f278f288b0e84, ; 538: lib_System.Net.Security.dll.so => 141
	i64 u0xfa5ed7226d978949, ; 539: lib-ar-Microsoft.Maui.Controls.resources.dll.so => 0
	i64 u0xfa645d91e9fc4cba, ; 540: System.Threading.Thread => 172
	i64 u0xfbad3e4ce4b98145, ; 541: System.Security.Cryptography.X509Certificates => 164
	i64 u0xfbf0a31c9fc34bc4, ; 542: lib_System.Net.Http.dll.so => 135
	i64 u0xfc6b7527cc280b3f, ; 543: lib_System.Runtime.Serialization.Formatters.dll.so => 158
	i64 u0xfc719aec26adf9d9, ; 544: Xamarin.AndroidX.Navigation.Fragment.dll => 96
	i64 u0xfd22f00870e40ae0, ; 545: lib_Xamarin.AndroidX.DrawerLayout.dll.so => 88
	i64 u0xfd49b3c1a76e2748, ; 546: System.Runtime.InteropServices.RuntimeInformation => 154
	i64 u0xfd4f1cb508086944, ; 547: Microsoft.Extensions.Http.Polly => 60
	i64 u0xfd536c702f64dc47, ; 548: System.Text.Encoding.Extensions => 166
	i64 u0xfd583f7657b6a1cb, ; 549: Xamarin.AndroidX.Fragment => 89
	i64 u0xfda36abccf05cf5c, ; 550: System.Net.WebSockets.Client => 144
	i64 u0xfeae9952cf03b8cb ; 551: tr/Microsoft.Maui.Controls.resources => 28
], align 8

@assembly_image_cache_indices = dso_local local_unnamed_addr constant [552 x i32] [
	i32 101, i32 72, i32 114, i32 51, i32 97, i32 182, i32 80, i32 108,
	i32 24, i32 2, i32 30, i32 139, i32 99, i32 113, i32 67, i32 31,
	i32 175, i32 83, i32 24, i32 111, i32 153, i32 88, i32 114, i32 63,
	i32 111, i32 78, i32 165, i32 170, i32 25, i32 107, i32 102, i32 76,
	i32 21, i32 183, i32 68, i32 53, i32 50, i32 137, i32 48, i32 136,
	i32 53, i32 87, i32 126, i32 163, i32 145, i32 152, i32 99, i32 8,
	i32 181, i32 9, i32 58, i32 145, i32 129, i32 131, i32 162, i32 179,
	i32 12, i32 167, i32 107, i32 18, i32 161, i32 35, i32 110, i32 178,
	i32 27, i32 182, i32 41, i32 47, i32 98, i32 16, i32 63, i32 126,
	i32 38, i32 121, i32 160, i32 75, i32 127, i32 151, i32 27, i32 131,
	i32 172, i32 38, i32 35, i32 118, i32 85, i32 159, i32 49, i32 41,
	i32 8, i32 105, i32 64, i32 13, i32 11, i32 145, i32 181, i32 139,
	i32 29, i32 138, i32 122, i32 7, i32 169, i32 125, i32 33, i32 20,
	i32 152, i32 149, i32 173, i32 26, i32 168, i32 129, i32 5, i32 121,
	i32 176, i32 59, i32 89, i32 34, i32 82, i32 123, i32 8, i32 176,
	i32 6, i32 44, i32 142, i32 67, i32 2, i32 65, i32 114, i32 103,
	i32 55, i32 152, i32 153, i32 39, i32 87, i32 137, i32 45, i32 102,
	i32 1, i32 73, i32 71, i32 70, i32 163, i32 166, i32 164, i32 46,
	i32 105, i32 175, i32 85, i32 47, i32 46, i32 74, i32 171, i32 95,
	i32 39, i32 81, i32 37, i32 77, i32 70, i32 179, i32 183, i32 20,
	i32 109, i32 159, i32 105, i32 40, i32 122, i32 24, i32 175, i32 52,
	i32 78, i32 22, i32 147, i32 98, i32 168, i32 75, i32 94, i32 138,
	i32 144, i32 132, i32 150, i32 75, i32 156, i32 14, i32 94, i32 182,
	i32 170, i32 1, i32 65, i32 52, i32 92, i32 124, i32 139, i32 52,
	i32 119, i32 85, i32 69, i32 108, i32 25, i32 138, i32 154, i32 31,
	i32 161, i32 160, i32 42, i32 90, i32 112, i32 54, i32 73, i32 148,
	i32 180, i32 120, i32 35, i32 15, i32 57, i32 109, i32 49, i32 78,
	i32 84, i32 173, i32 36, i32 117, i32 3, i32 61, i32 143, i32 155,
	i32 83, i32 112, i32 167, i32 115, i32 136, i32 72, i32 176, i32 119,
	i32 5, i32 57, i32 106, i32 134, i32 66, i32 4, i32 156, i32 180,
	i32 104, i32 162, i32 65, i32 157, i32 118, i32 92, i32 86, i32 3,
	i32 123, i32 125, i32 9, i32 155, i32 18, i32 40, i32 69, i32 64,
	i32 86, i32 64, i32 96, i32 67, i32 2, i32 54, i32 129, i32 28,
	i32 18, i32 14, i32 115, i32 11, i32 127, i32 134, i32 60, i32 55,
	i32 100, i32 157, i32 17, i32 27, i32 89, i32 59, i32 7, i32 116,
	i32 25, i32 4, i32 17, i32 146, i32 113, i32 161, i32 147, i32 117,
	i32 102, i32 56, i32 91, i32 178, i32 33, i32 80, i32 82, i32 124,
	i32 29, i32 32, i32 131, i32 33, i32 55, i32 172, i32 126, i32 51,
	i32 68, i32 106, i32 179, i32 115, i32 154, i32 94, i32 120, i32 121,
	i32 9, i32 36, i32 86, i32 173, i32 110, i32 50, i32 71, i32 95,
	i32 10, i32 23, i32 22, i32 21, i32 122, i32 34, i32 128, i32 171,
	i32 92, i32 66, i32 87, i32 168, i32 54, i32 133, i32 1, i32 17,
	i32 128, i32 6, i32 42, i32 13, i32 69, i32 117, i32 110, i32 132,
	i32 97, i32 16, i32 164, i32 73, i32 79, i32 56, i32 19, i32 95,
	i32 74, i32 91, i32 165, i32 104, i32 98, i32 130, i32 43, i32 45,
	i32 16, i32 119, i32 158, i32 146, i32 165, i32 100, i32 88, i32 90,
	i32 12, i32 37, i32 45, i32 62, i32 43, i32 150, i32 135, i32 58,
	i32 5, i32 132, i32 157, i32 91, i32 47, i32 177, i32 23, i32 170,
	i32 19, i32 174, i32 116, i32 141, i32 181, i32 147, i32 93, i32 26,
	i32 169, i32 3, i32 38, i32 82, i32 44, i32 10, i32 0, i32 130,
	i32 62, i32 123, i32 26, i32 180, i32 59, i32 77, i32 50, i32 109,
	i32 22, i32 15, i32 177, i32 113, i32 39, i32 142, i32 158, i32 48,
	i32 135, i32 136, i32 103, i32 127, i32 81, i32 0, i32 118, i32 133,
	i32 49, i32 79, i32 42, i32 15, i32 103, i32 144, i32 93, i32 116,
	i32 140, i32 96, i32 150, i32 148, i32 162, i32 41, i32 166, i32 174,
	i32 112, i32 28, i32 20, i32 23, i32 34, i32 53, i32 169, i32 106,
	i32 107, i32 32, i32 140, i32 101, i32 120, i32 149, i32 90, i32 155,
	i32 81, i32 148, i32 61, i32 14, i32 58, i32 93, i32 46, i32 100,
	i32 101, i32 56, i32 62, i32 76, i32 108, i32 57, i32 111, i32 30,
	i32 151, i32 30, i32 133, i32 40, i32 61, i32 160, i32 167, i32 32,
	i32 72, i32 177, i32 171, i32 48, i32 11, i32 80, i32 84, i32 163,
	i32 130, i32 156, i32 149, i32 141, i32 140, i32 13, i32 21, i32 12,
	i32 44, i32 7, i32 159, i32 183, i32 74, i32 37, i32 124, i32 51,
	i32 77, i32 36, i32 142, i32 134, i32 97, i32 70, i32 83, i32 68,
	i32 79, i32 76, i32 6, i32 137, i32 143, i32 66, i32 125, i32 19,
	i32 104, i32 178, i32 151, i32 43, i32 128, i32 143, i32 84, i32 4,
	i32 146, i32 31, i32 71, i32 29, i32 60, i32 153, i32 99, i32 174,
	i32 10, i32 63, i32 141, i32 0, i32 172, i32 164, i32 135, i32 158,
	i32 96, i32 88, i32 154, i32 60, i32 166, i32 89, i32 144, i32 28
], align 4

@marshal_methods_number_of_classes = dso_local local_unnamed_addr constant i32 0, align 4

@marshal_methods_class_cache = dso_local local_unnamed_addr global [0 x %struct.MarshalMethodsManagedClass] zeroinitializer, align 8

; Names of classes in which marshal methods reside
@mm_class_names = dso_local local_unnamed_addr constant [0 x ptr] zeroinitializer, align 8

@mm_method_names = dso_local local_unnamed_addr constant [1 x %struct.MarshalMethodName] [
	%struct.MarshalMethodName {
		i64 u0x0000000000000000, ; name: 
		ptr @.MarshalMethodName.0_name; char* name
	} ; 0
], align 8

; get_function_pointer (uint32_t mono_image_index, uint32_t class_index, uint32_t method_token, void*& target_ptr)
@get_function_pointer = internal dso_local unnamed_addr global ptr null, align 8

; Functions

; Function attributes: memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nofree norecurse nosync nounwind "stack-protector-buffer-size"="8" uwtable willreturn
define void @xamarin_app_init(ptr nocapture noundef readnone %env, ptr noundef %fn) local_unnamed_addr #0
{
	%fnIsNull = icmp eq ptr %fn, null
	br i1 %fnIsNull, label %1, label %2

1: ; preds = %0
	%putsResult = call noundef i32 @puts(ptr @.str.0)
	call void @abort()
	unreachable 

2: ; preds = %1, %0
	store ptr %fn, ptr @get_function_pointer, align 8, !tbaa !3
	ret void
}

; Strings
@.str.0 = private unnamed_addr constant [40 x i8] c"get_function_pointer MUST be specified\0A\00", align 1

;MarshalMethodName
@.MarshalMethodName.0_name = private unnamed_addr constant [1 x i8] c"\00", align 1

; External functions

; Function attributes: "no-trapping-math"="true" noreturn nounwind "stack-protector-buffer-size"="8"
declare void @abort() local_unnamed_addr #2

; Function attributes: nofree nounwind
declare noundef i32 @puts(ptr noundef) local_unnamed_addr #1
attributes #0 = { memory(write, argmem: none, inaccessiblemem: none) "min-legal-vector-width"="0" mustprogress "no-trapping-math"="true" nofree norecurse nosync nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" uwtable willreturn }
attributes #1 = { nofree nounwind }
attributes #2 = { "no-trapping-math"="true" noreturn nounwind "stack-protector-buffer-size"="8" "target-cpu"="generic" "target-features"="+fix-cortex-a53-835769,+neon,+outline-atomics,+v8a" }

; Metadata
!llvm.module.flags = !{!0, !1, !7, !8, !9, !10}
!0 = !{i32 1, !"wchar_size", i32 4}
!1 = !{i32 7, !"PIC Level", i32 2}
!llvm.ident = !{!2}
!2 = !{!".NET for Android remotes/origin/release/9.0.1xx @ e7876a4f92d894b40c191a24c2b74f06d4bf4573"}
!3 = !{!4, !4, i64 0}
!4 = !{!"any pointer", !5, i64 0}
!5 = !{!"omnipotent char", !6, i64 0}
!6 = !{!"Simple C++ TBAA"}
!7 = !{i32 1, !"branch-target-enforcement", i32 0}
!8 = !{i32 1, !"sign-return-address", i32 0}
!9 = !{i32 1, !"sign-return-address-all", i32 0}
!10 = !{i32 1, !"sign-return-address-with-bkey", i32 0}
