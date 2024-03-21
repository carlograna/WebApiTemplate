namespace WebApiTemplate.Documentacion
{
    API USANDO BASE DE DATOS, NLOG, JWT(JSON WEBTOKEN) Y VALIDACIONES

    PAQUETES NECESARIOS PARA LA INSTALACION DE JWT:

    Microsoft.AspNetCore.Authentication.JwtBearer V6.0.9
    System.IdentityModel.Tokens.Jwt V6.10.0


    PAQUETES UTILIZADOS PARA USAR NLOG:

    NLog V5.2.3
    NLog.Web.AspNetCore V5.15

    PAQUETES UTILIZADOS PARA USAR EN  LA BASE DE DATOS USANDOO ENTITY FRAMEWORK:

    Microsoft.EntityFrameworkCore V6.0.22
    Microsoft.EntityFrameworkCore.Design V6.0.22



    Documentación de Login con Google y Facebook

Google
-Ingresamos a la consola de google: Consola de Desarrolladores de Google
- Crea un nuevo proyecto o selecciona uno existente
-Activa la API de Google + API y configura tus credenciales OAuth 2.0.
  Esto proporcionara un ID de cliente que necesitaras en tu aplicacion angular.
Configuraciones en angular
-Instalar las dependencias necesarias: Instala el paquete angularx-social-login u otra
 librería similar que facilite la autenticación social en Angular.
 npm install angularx-social-login
- Configurar la autenticación en tu aplicación Angular:
 Importa el módulo de autenticación social en tu módulo raíz.
 
 import { SocialLoginModule, SocialAuthServiceConfig } from 'angularx-social-login';
import { GoogleLoginProvider } from 'angularx-social-login';

@NgModule({
  imports: [
    ...
    SocialLoginModule
  ],
  providers: [
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: GoogleLoginProvider.PROVIDER_ID,
            provider: new GoogleLoginProvider('Tu-ID-de-cliente')
          }
        ]
      } as SocialAuthServiceConfig,
    }
  ],
  ...
})
export class AppModule { }

- Crear un servicio de autenticación:
Crea un servicio para manejar la lógica de autenticación.

import { Injectable } from '@angular/core';
import { SocialAuthService, SocialUser } from 'angularx-social-login';
import { GoogleLoginProvider } from 'angularx-social-login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private authService: SocialAuthService) { }

  signInWithGoogle(): void {
    this.authService.signIn(GoogleLoginProvider.PROVIDER_ID);
  }

  signOut(): void {
    this.authService.signOut();
  }

  // Puedes añadir más lógica aquí según tus necesidades
}

- Utilizar la autenticación en tu componente:
En el componente donde desees implementar el inicio de sesión con Google, 
puedes inyectar el servicio de autenticación y llamar a los métodos apropiados.

Para ayudar diríjase al siguiente video: https://www.youtube.com/watch?v=0PHS5R5uYzk

Facebook
- Ve al Panel de Desarrolladores de Facebook y crea una nueva aplicación.
- Crea una nueva aplicación o selecciona una existente en el panel de control
  del desarrollador.
 - En la sección "Configuración" de tu aplicación, agrega tu dominio de Angular
 (por ejemplo, http://localhost:4200) en la sección "URI de redireccionamiento de
 OAuth válidos".
   Configuraciones en angular
 - Instalar las dependencias necesarias: 
   Instala el paquete angularx-social-login u otra
	librería similar que facilite la autenticación social en Angular.
	npm install angularx-social-login
 - Configurar la autenticación en tu aplicación Angular:
   Importa los módulos necesarios y configura las credenciales 
   de Facebook en tu módulo raíz.
   
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { SocialLoginModule, SocialAuthServiceConfig } from 'angularx-social-login';
import { FacebookLoginProvider } from 'angularx-social-login';
import { AppComponent } from './app.component';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    SocialLoginModule
  ],
  providers: [
    {
      provide: 'SocialAuthServiceConfig',
      useValue: {
        autoLogin: false,
        providers: [
          {
            id: FacebookLoginProvider.PROVIDER_ID,
            provider: new FacebookLoginProvider('Tu-App-ID')
          }
        ]
      } as SocialAuthServiceConfig,
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

- Crear un servicio de autenticación:
  Crea un servicio para manejar la lógica de autenticación.
  
 import { Injectable } from '@angular/core';
import { SocialAuthService, SocialUser } from 'angularx-social-login';
import { FacebookLoginProvider } from 'angularx-social-login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private authService: SocialAuthService) { }

  signInWithFacebook(): void {
    this.authService.signIn(FacebookLoginProvider.PROVIDER_ID);
  }

  signOut(): void {
    this.authService.signOut();
  }

  // Puedes añadir más lógica aquí según tus necesidades
}


- Utilizar la autenticación en tu componente:

En el componente donde desees implementar el inicio de sesión con Facebook,
 inyecta el servicio de autenticación y llama a los métodos apropiados.
Para más información dirijas al siguiente documento: https://www.npmjs.com/package/@abacritt/angularx-social-login



}
