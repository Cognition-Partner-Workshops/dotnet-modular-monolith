package com.mycompany.mymeetings;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.scheduling.annotation.EnableScheduling;

@SpringBootApplication
@EnableScheduling
public class AdministrationModuleApplication {

    public static void main(String[] args) {
        SpringApplication.run(AdministrationModuleApplication.class, args);
    }
}
