import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './layouts/layout.component';
import { AuthGuard } from './+auth/helpers';


const routes: Routes = [
  { path: 'auth', loadChildren: () => import('./+auth/auth.module').then(m => m.AuthModule) },
  // {
  //   path: 'dashboard',
  //   canActivate: [AuthGuard],
  //   component: LayoutComponent,
  //   loadChildren: () => import('./+dashboard/dashboard.module').then(m => m.DashboardModule)
  // },
  {
    path: 'users',
    canActivate: [AuthGuard],
    component: LayoutComponent,
    loadChildren: () => import('./+users/users.module').then(m => m.UsersModule)
  },

  {
    path: '',
    redirectTo: '/users',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: '/users' //Error 404 - Page not found
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
