// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Proto/game.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace GameServiceNamespace {
  public static partial class GameServiceProto
  {
    static readonly string __ServiceName = "GameServiceProto";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GameServiceNamespace.EmptyRequest> __Marshaller_EmptyRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GameServiceNamespace.EmptyRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GameServiceNamespace.GameList> __Marshaller_GameList = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GameServiceNamespace.GameList.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GameServiceNamespace.JoinGameRequest> __Marshaller_JoinGameRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GameServiceNamespace.JoinGameRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GameServiceNamespace.GameResult> __Marshaller_GameResult = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GameServiceNamespace.GameResult.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GameServiceNamespace.EmptyRequest, global::GameServiceNamespace.GameList> __Method_GetGames = new grpc::Method<global::GameServiceNamespace.EmptyRequest, global::GameServiceNamespace.GameList>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetGames",
        __Marshaller_EmptyRequest,
        __Marshaller_GameList);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GameServiceNamespace.JoinGameRequest, global::GameServiceNamespace.GameResult> __Method_JoinGame = new grpc::Method<global::GameServiceNamespace.JoinGameRequest, global::GameServiceNamespace.GameResult>(
        grpc::MethodType.Unary,
        __ServiceName,
        "JoinGame",
        __Marshaller_JoinGameRequest,
        __Marshaller_GameResult);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::GameServiceNamespace.GameReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of GameServiceProto</summary>
    [grpc::BindServiceMethod(typeof(GameServiceProto), "BindService")]
    public abstract partial class GameServiceProtoBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::GameServiceNamespace.GameList> GetGames(global::GameServiceNamespace.EmptyRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::GameServiceNamespace.GameResult> JoinGame(global::GameServiceNamespace.JoinGameRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(GameServiceProtoBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_GetGames, serviceImpl.GetGames)
          .AddMethod(__Method_JoinGame, serviceImpl.JoinGame).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, GameServiceProtoBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_GetGames, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GameServiceNamespace.EmptyRequest, global::GameServiceNamespace.GameList>(serviceImpl.GetGames));
      serviceBinder.AddMethod(__Method_JoinGame, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::GameServiceNamespace.JoinGameRequest, global::GameServiceNamespace.GameResult>(serviceImpl.JoinGame));
    }

  }
}
#endregion
