﻿using Microsoft.Extensions.Configuration;
using System;

namespace RockLib.Secrets
{
    /// <summary>
    /// Extension methods for adding <see cref="SecretsConfigurationProvider"/>.
    /// </summary>
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>
        /// The key to the <see cref="IConfigurationBuilder.Properties"/> where the
        /// default default action to be invoked for SecretsConfigurationProvider when
        /// an error occurs while getting the value of a secret is stored.
        /// </summary>
        public const string SecretExceptionHandlerKey = "RockLib.SecretExceptionHandler";

        /// <summary>
        /// Adds a secrets configuration source to the <paramref name="builder"/>, returning
        /// an <see cref="ISecretsConfigurationBuilder"/> used to define the source's secrets.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <returns>
        /// An <see cref="ISecretsConfigurationBuilder"/> used to define the source's secrets.
        /// </returns>
        public static ISecretsConfigurationBuilder AddRockLibSecrets(this IConfigurationBuilder builder) =>
            builder.AddRockLibSecrets(null);

        /// <summary>
        /// Adds a secrets configuration source to the <paramref name="builder"/>, returning
        /// an <see cref="ISecretsConfigurationBuilder"/> used to define the source's secrets.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">
        /// Configures the secrets configuration source. Can be <see langword="null"/>.
        /// </param>
        /// <returns>
        /// An <see cref="ISecretsConfigurationBuilder"/> used to define the source's secrets.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="builder"/> is <c>null</c>.
        /// </exception>
        public static ISecretsConfigurationBuilder AddRockLibSecrets(this IConfigurationBuilder builder,
            Action<SecretsConfigurationSource>? configureSource)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var source = new SecretsConfigurationSource();
            configureSource?.Invoke(source);
            builder.Add(source);
            return new SecretsConfigurationBuilder(source);
        }

        /// <summary>
        /// Sets a default action to be invoked for SecretsConfigurationProvider when an error occurs while getting
        /// the value of a secret.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="onSecretException">The action to be invoked for SecretsConfigurationProvider.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="builder"/> or <paramref name="onSecretException"/> is <c>null</c>.
        /// </exception>
        public static IConfigurationBuilder SetSecretExceptionHandler(this IConfigurationBuilder builder, 
            Action<SecretExceptionContext> onSecretException)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if(onSecretException is null)
            {
                throw new ArgumentNullException(nameof(onSecretException));
            }

            builder.Properties[SecretExceptionHandlerKey] = onSecretException;
            return builder;
        }

        /// <summary>
        /// Gets the default action to be invoked for SecretsConfigurationProvider when an error occurs while
        /// getting the value of a secret.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>
        /// The default action to be invoked for SecretsConfigurationProvider when an error occurs while
        /// getting the value of a secret, or <c>null</c> otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="builder"/> is <c>null</c>.
        /// </exception>
        public static Action<SecretExceptionContext>? GetSecretExceptionHandler(this IConfigurationBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builder.Properties.TryGetValue(SecretExceptionHandlerKey, out var value) && value is Action<SecretExceptionContext> exceptionHandler)
            {
                return exceptionHandler;
            }

            return null;
        }
    }
}