using System;
using Leopotam.EcsLite;

namespace OTUS_Education.Assets.Homeworks.Homework_7.Scripts.Services
{
    public struct UnpackEntityUtils
    {
        public static int UnpackEntity(EcsWorld world, EcsPackedEntity pack)
        {
            if (pack.Unpack(world, out int entity))
            {
                return entity;
            }
            else
            {
                throw new InvalidOperationException("Failed to unpack!");
            }
        }

        public static (int, int) UnpackEntity(EcsWorld world, EcsPackedEntity pack1, EcsPackedEntity pack2)
        {
            if (pack1.Unpack(world, out int entity1) && pack2.Unpack(world, out int entity2))
            {
                return (entity1, entity2);
            }
            else
            {
                throw new InvalidOperationException("Failed to unpack!");
            }
        }
    }
}