import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  notify(action: string): void {
    window.alert(`${action} selected. Connect this action to your Angular route or API endpoint.`);
  }
}
