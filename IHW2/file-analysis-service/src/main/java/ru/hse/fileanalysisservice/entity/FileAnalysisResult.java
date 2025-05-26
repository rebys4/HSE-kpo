package ru.hse.fileanalysisservice.entity;


import jakarta.persistence.*;
import lombok.AllArgsConstructor;
import lombok.Builder;
import lombok.Data;
import lombok.NoArgsConstructor;

import java.time.LocalDateTime;

@Entity
@Data
@NoArgsConstructor
@AllArgsConstructor
@Builder
public class FileAnalysisResult {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;

    private Long fileId;

    @Column(length = 2048)
    private String fileName;

    private int paragraphsCount;
    private int wordsCount;
    private int charactersCount;

    @Column(columnDefinition = "text", length = 1024)
    private String wordCloudUrl;
    private LocalDateTime analyzedAt;
}