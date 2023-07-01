import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddFeatureComponent } from './add-feature/add-feature.component';
import { ConfigureFeatureComponent } from './configure-feature/configure-feature.component';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';


@NgModule({
  declarations: [
    AddFeatureComponent,
    ConfigureFeatureComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    NgSelectModule,
    RouterModule.forRoot([
      { path: 'OTA', component: AddFeatureComponent, pathMatch: 'full' },
      { path: 'OTA/config', component: ConfigureFeatureComponent, pathMatch: 'full' },
      //{ path: 'counter', component: CounterComponent },
      //{ path: 'fetch-data', component: FetchDataComponent, canActivate: [AuthorizeGuard] },
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ]
})
export class OTAModule { }
