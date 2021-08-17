import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ListComponent } from './list/list.component';
import { MemberDetailsComponent } from './members/member-details/member-details.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedGuard } from './_guards/prevent-unsaved.guard';

const routes: Routes = [
  {path: "", component: HomeComponent},
  {
    path: "",
    runGuardsAndResolvers: "always",
    canActivate: [AuthGuard],
    children: [
      {path: "members", component: MemberListComponent},
      {path: "members/:username", component: MemberDetailsComponent},
      {path: "lists", component: ListComponent},
      {path: "messages", component: MessagesComponent},
      {path: "member/edit", component: MemberEditComponent, canDeactivate: [PreventUnsavedGuard]}
    ]
  },
  {path: "not-found", component: NotFoundComponent},
  {path: "server-error", component: ServerErrorComponent},
  {path: "**", component: HomeComponent, pathMatch: "full"}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
