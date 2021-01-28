﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;


namespace Shiny.BluetoothLE
{
    public static class AsyncExtensions
    {
        public static Task ConnectWaitAsync(this IPeripheral peripheral, CancellationToken? cancelToken = null)
            => peripheral
                .ConnectWait()
                .Timeout(TimeSpan.FromSeconds(30))
                .ToTask(cancelToken ?? CancellationToken.None);

        public static Task ConnectWaitAsync(this IPeripheral peripheral, CancellationToken? cancelToken = null, ConnectionConfig? connectionConfig = null)
            => peripheral
                .ConnectWait(connectionConfig ?? null)
                .Timeout(TimeSpan.FromSeconds(30))
                .ToTask(cancelToken ?? CancellationToken.None);

        public static Task<IList<IGattService>> GetServicesAsync(this IPeripheral peripheral, CancellationToken? cancelToken = null)
            => peripheral
                .DiscoverServices()
                .ToList()
                .ToTask(cancelToken ?? CancellationToken.None);


        public static Task<IList<IGattCharacteristic>> GetCharacteristicsAsync(this IGattService service, CancellationToken? cancelToken = null)
            => service
                .DiscoverCharacteristics()
                .ToList()
                .ToTask(cancelToken ?? CancellationToken.None);


        public static Task<IList<IGattCharacteristic>> GetCharacteristicsAsync(this IPeripheral peripheral, Guid serviceUuid, CancellationToken? cancelToken = null)
            => peripheral
                .GetCharacteristicsForService(serviceUuid)
                .ToList()
                .ToTask(cancelToken ?? CancellationToken.None);


        public static Task<IList<IGattCharacteristic>> GetAllCharacteristicsAsync(this IPeripheral peripheral, CancellationToken? cancelToken = null)
            => peripheral
                .WhenAnyCharacteristicDiscovered()
                .ToList()
                .ToTask(cancelToken ?? CancellationToken.None);


        public static Task<IGattCharacteristic> GetKnownCharacteristicAsync(this IPeripheral peripheral, Guid serviceUuid, Guid characteristicUuid, CancellationToken? cancelToken = null)
            => peripheral
                .GetKnownCharacteristics(serviceUuid, characteristicUuid)
                .Take(1)
                .ToTask(cancelToken ?? CancellationToken.None);


        public static Task<CharacteristicGattResult> WriteAsync(this IGattCharacteristic characteristic, byte[] data, bool withResponse, CancellationToken? cancelToken = null)
            => characteristic.Write(data, withResponse).ToTask(cancelToken ?? CancellationToken.None);


        public static Task<CharacteristicGattResult> ReadAsync(this IGattCharacteristic characteristic, CancellationToken? cancelToken = null)
            => characteristic.Read().ToTask(cancelToken ?? CancellationToken.None);
    }
}
