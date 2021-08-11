import { Inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LocalStorageService {

  private storage: Storage;
  constructor() {
    this.storage = window.localStorage;
   }

    public setValue(key: string, value: string): void {
        this.storage.setItem(key, value);
    }

    public getValue(key: string): string {
      let value = this.storage.getItem(key);
      if (value != null)
        return value
      else
        return '';
    }

    public deleteValue(key: string): void {
        this.storage.removeItem('key');
    }
}
