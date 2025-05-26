package ru.hse.filestoringservice.controller;

import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.media.Content;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import lombok.RequiredArgsConstructor;
import org.springframework.core.io.*;
import org.springframework.http.*;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.multipart.MultipartFile;
import ru.hse.filestoringservice.entity.StoredFile;
import ru.hse.filestoringservice.service.FileStorageService;

import java.nio.file.Files;

@RestController
@RequestMapping("/files")
@RequiredArgsConstructor
public class FileController {

    private final FileStorageService fileStorageService;

    @Operation(summary = "Загрузить файл")
    @ApiResponses({
            @ApiResponse(responseCode = "200", description = "Файл успешно загружен"),
            @ApiResponse(responseCode = "500", description = "Ошибка сервера")
    })
    @PostMapping(consumes = MediaType.MULTIPART_FORM_DATA_VALUE)
    public ResponseEntity<StoredFile> uploadFile(
            @Parameter(description = "Загружаемый файл", content = @Content(mediaType = MediaType.MULTIPART_FORM_DATA_VALUE))
            @RequestParam("file") MultipartFile file) {
        try {
            StoredFile metadata = fileStorageService.saveFile(file);
            return ResponseEntity.ok(metadata);
        } catch (Exception e) {
            e.printStackTrace();
            return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).build();
        }
    }

    @Operation(summary = "Получить файл по id")
    @GetMapping("/{id}")
    public ResponseEntity<Resource> downloadFile(@PathVariable Long id) {
        return fileStorageService.getFileMetadata(id)
                .map(meta -> {
                    try {
                        Resource resource = new FileSystemResource(fileStorageService.getFilePath(meta));
                        String contentType = meta.getContentType() != null ? meta.getContentType() : Files.probeContentType(fileStorageService.getFilePath(meta));
                        return ResponseEntity.ok()
                                .contentType(MediaType.parseMediaType(contentType))
                                .header(HttpHeaders.CONTENT_DISPOSITION, "attachment; filename=\"" + meta.getOriginalName() + "\"")
                                .body(resource);
                    } catch (Exception e) {
                        e.printStackTrace();
                        return ResponseEntity.status(HttpStatus.INTERNAL_SERVER_ERROR).<Resource>build();
                    }
                })
                .orElse(ResponseEntity.notFound().<Resource>build());
    }
}